#!/usr/bin/env python3
"""Generate event-based synthetic data for ECOS contract-renewal modelling.

The generator intentionally creates business events first, then derives exactly one
T-60 snapshot per contract. Renewal is sampled from a hidden, stochastic decision
process that also sees post-cutoff events. It is therefore not a deterministic label
formula over the exported model features.

Only Python's standard library is required.
"""

from __future__ import annotations

import argparse
import calendar
import csv
import hashlib
import json
import math
import random
import statistics
from collections import Counter
from dataclasses import asdict, dataclass
from datetime import date, datetime, timedelta, timezone
from pathlib import Path
from typing import Any, Iterable, Sequence


SCRIPT_DIR = Path(__file__).resolve().parent

FREQUENCY_MONTHS = {
    "TSQT01": None,  # Không định kỳ: một đợt trong hợp đồng
    "TSQT02": 6,
    "TSQT03": 3,
}

SECTORS = {
    "san_xuat": {"need": 0.70, "complexity": 0.62},
    "det_nhuom": {"need": 0.82, "complexity": 0.78},
    "thuc_pham": {"need": 0.68, "complexity": 0.55},
    "hoa_chat": {"need": 0.90, "complexity": 0.86},
    "khu_cong_nghiep": {"need": 0.88, "complexity": 0.80},
    "chan_nuoi": {"need": 0.72, "complexity": 0.60},
    "vat_lieu_xay_dung": {"need": 0.64, "complexity": 0.58},
    "y_te": {"need": 0.76, "complexity": 0.66},
    "logistics": {"need": 0.42, "complexity": 0.38},
}

PROVINCES = (
    "TP.HCM",
    "Binh Duong",
    "Dong Nai",
    "Long An",
    "Ba Ria - Vung Tau",
    "Tay Ninh",
    "Tien Giang",
    "Can Tho",
)

COMPANY_SIZES = {
    "small": {"weight": 0.34, "coordination": -0.06, "budget": 0.78},
    "medium": {"weight": 0.46, "coordination": 0.03, "budget": 1.00},
    "large": {"weight": 0.20, "coordination": 0.10, "budget": 1.35},
}

ARCHETYPES = {
    "stable": {
        "weight": 0.34,
        "loyalty": (3.6, 2.0),
        "price": (2.0, 3.8),
        "health": (4.2, 1.8),
        "volatility": (0.08, 0.20),
    },
    "price_sensitive": {
        "weight": 0.23,
        "loyalty": (2.4, 3.0),
        "price": (4.5, 1.8),
        "health": (3.1, 2.4),
        "volatility": (0.15, 0.34),
    },
    "regulated": {
        "weight": 0.24,
        "loyalty": (3.2, 2.3),
        "price": (2.2, 3.6),
        "health": (3.6, 2.0),
        "volatility": (0.08, 0.22),
    },
    "volatile": {
        "weight": 0.19,
        "loyalty": (2.1, 2.8),
        "price": (3.0, 2.4),
        "health": (2.5, 2.8),
        "volatility": (0.28, 0.55),
    },
}


MODEL_FEATURES_CATEGORICAL = [
    "frequency_code",
]

MODEL_FEATURES_NUMERIC = [
    "contract_sequence_number",
    "contract_duration_months",
    "days_observed_current_contract",
    "expected_rounds_total",
    "rounds_due_by_cutoff",
    "rounds_completed_by_cutoff",
    "open_overdue_rounds_at_cutoff",
    "completion_rate_to_cutoff",
    "on_time_rate_completed",
    "average_delay_days",
    "maximum_delay_days",
    "average_processing_days",
    "maximum_processing_days",
    "recent_90d_completed_rounds",
    "recent_90d_average_delay_days",
    "has_customer_history",
    "previous_contract_count",
    "relationship_tenure_days",
    "days_since_previous_contract_end",
    "historical_rounds_completed",
    "historical_completion_rate",
    "historical_on_time_rate",
    "historical_average_delay_days",
    "historical_average_processing_days",
    "historical_renewal_rate",
    "current_metrics_available",
    "historical_metrics_available",
]

MODEL_FEATURES = MODEL_FEATURES_CATEGORICAL + MODEL_FEATURES_NUMERIC

SNAPSHOT_METADATA_COLUMNS = [
    "snapshot_id",
    "customer_id",
    "contract_id",
    "snapshot_date",
    "contract_end_date",
    "latest_observed_event_date",
    "label_window_end",
    "label_observed",
    "split",
]

LABEL_COLUMN = "renewed_with_new_contract"


@dataclass(frozen=True)
class GeneratorConfig:
    seed: int
    customer_count: int
    simulation_start: str
    simulation_end: str
    prediction_lead_days: int
    label_observation_days: int
    max_contracts_per_customer: int
    include_latent_debug: bool
    output_directory: str

    @classmethod
    def from_json(cls, path: Path) -> "GeneratorConfig":
        with path.open("r", encoding="utf-8") as handle:
            raw = json.load(handle)
        config = cls(**raw)
        config.validate()
        return config

    def validate(self) -> None:
        start = parse_date(self.simulation_start)
        end = parse_date(self.simulation_end)
        if start >= end:
            raise ValueError("simulation_start must be before simulation_end")
        if self.customer_count < 20:
            raise ValueError("customer_count must be at least 20")
        if not 15 <= self.prediction_lead_days <= 180:
            raise ValueError("prediction_lead_days must be between 15 and 180")
        if not 30 <= self.label_observation_days <= 365:
            raise ValueError("label_observation_days must be between 30 and 365")
        if not 1 <= self.max_contracts_per_customer <= 20:
            raise ValueError("max_contracts_per_customer must be between 1 and 20")


@dataclass
class CustomerProfile:
    customer_id: str
    company_name: str
    sector: str
    company_size: str
    province: str
    created_date: date
    archetype: str
    compliance_need: float
    loyalty: float
    price_sensitivity: float
    business_health: float
    coordination_quality: float
    relationship_baseline: float
    decision_volatility: float
    feedback_propensity: float


def parse_date(value: str) -> date:
    return date.fromisoformat(value)


def iso(value: date | None) -> str:
    return value.isoformat() if value else ""


def clamp(value: float, lower: float, upper: float) -> float:
    return max(lower, min(upper, value))


def add_months(value: date, months: int) -> date:
    month_index = value.month - 1 + months
    year = value.year + month_index // 12
    month = month_index % 12 + 1
    day = min(value.day, calendar.monthrange(year, month)[1])
    return date(year, month, day)


def mean_or_none(values: Sequence[float | int]) -> float | None:
    return round(statistics.fmean(values), 4) if values else None


def ratio(numerator: int | float, denominator: int | float) -> float:
    if not denominator:
        return 0.0
    return round(float(numerator) / float(denominator), 4)


def gumbel(rng: random.Random) -> float:
    uniform = clamp(rng.random(), 1e-12, 1 - 1e-12)
    return -math.log(-math.log(uniform))


def poisson(rng: random.Random, lam: float, cap: int = 8) -> int:
    lam = clamp(lam, 0.0, 5.0)
    threshold = math.exp(-lam)
    product = 1.0
    count = 0
    while product > threshold and count <= cap:
        count += 1
        product *= rng.random()
    return min(cap, max(0, count - 1))


def weighted_choice(rng: random.Random, choices: Sequence[tuple[Any, float]]) -> Any:
    total = sum(weight for _, weight in choices)
    point = rng.random() * total
    cursor = 0.0
    for value, weight in choices:
        cursor += weight
        if point <= cursor:
            return value
    return choices[-1][0]


class SyntheticRenewalGenerator:
    def __init__(self, config: GeneratorConfig) -> None:
        self.config = config
        self.rng = random.Random(config.seed)
        self.simulation_start = parse_date(config.simulation_start)
        self.simulation_end = parse_date(config.simulation_end)
        self.customers: list[dict[str, Any]] = []
        self.contracts: list[dict[str, Any]] = []
        self.monitoring_rounds: list[dict[str, Any]] = []
        self.snapshots: list[dict[str, Any]] = []
        self.latent_debug: list[dict[str, Any]] = []
        self._contract_counter = 0
        self._round_counter = 0
        self._contract_rows_by_id: dict[str, dict[str, Any]] = {}

    def generate(self) -> None:
        for customer_number in range(1, self.config.customer_count + 1):
            profile = self._create_customer(customer_number)
            self._generate_customer_history(profile)
        self._assign_time_splits()
        self._validate_in_memory()

    def _create_customer(self, number: int) -> CustomerProfile:
        customer_id = f"KH_SYN_{number:05d}"
        sector = weighted_choice(
            self.rng,
            [(name, 1.0) for name in SECTORS],
        )
        company_size = weighted_choice(
            self.rng,
            [(name, values["weight"]) for name, values in COMPANY_SIZES.items()],
        )
        archetype = weighted_choice(
            self.rng,
            [(name, values["weight"]) for name, values in ARCHETYPES.items()],
        )
        archetype_values = ARCHETYPES[archetype]
        sector_values = SECTORS[sector]
        size_values = COMPANY_SIZES[company_size]

        compliance_need = clamp(
            0.55 * sector_values["need"]
            + 0.45 * self.rng.betavariate(3.5, 2.0),
            0.05,
            0.98,
        )
        if archetype == "regulated":
            compliance_need = clamp(compliance_need + 0.12, 0.05, 0.99)

        # New customers continue to enter the portfolio throughout the simulation.
        # Keeping all acquisition in the first third created an unrealistic time
        # split: almost every late snapshot belonged to a long-tenure customer.
        latest_created = self.simulation_end - timedelta(
            days=365 + self.config.label_observation_days
        )
        created_offset = self.rng.randint(
            0,
            max(1, (latest_created - self.simulation_start).days),
        )
        created_date = self.simulation_start + timedelta(days=created_offset)
        profile = CustomerProfile(
            customer_id=customer_id,
            company_name=f"Doanh nghiep synthetic {sector.replace('_', ' ')} {number:05d}",
            sector=sector,
            company_size=company_size,
            province=self.rng.choice(PROVINCES),
            created_date=created_date,
            archetype=archetype,
            compliance_need=compliance_need,
            loyalty=self.rng.betavariate(*archetype_values["loyalty"]),
            price_sensitivity=self.rng.betavariate(*archetype_values["price"]),
            business_health=self.rng.betavariate(*archetype_values["health"]),
            coordination_quality=clamp(
                self.rng.betavariate(3.0, 2.2) + size_values["coordination"],
                0.03,
                0.98,
            ),
            relationship_baseline=self.rng.betavariate(3.2, 2.5),
            decision_volatility=self.rng.uniform(*archetype_values["volatility"]),
            feedback_propensity=self.rng.betavariate(2.2, 2.6),
        )
        self.customers.append(
            {
                "customer_id": profile.customer_id,
                "company_name": profile.company_name,
                "sector": profile.sector,
                "company_size": profile.company_size,
                "province": profile.province,
                "created_date": iso(profile.created_date),
                "is_synthetic": 1,
            }
        )
        if self.config.include_latent_debug:
            hidden = asdict(profile)
            hidden["created_date"] = iso(profile.created_date)
            self.latent_debug.append(hidden)
        return profile

    def _generate_customer_history(self, profile: CustomerProfile) -> None:
        max_first_start = min(
            self.simulation_end - timedelta(days=365),
            profile.created_date + timedelta(days=365 * 2),
        )
        first_window = max(1, (max_first_start - profile.created_date).days)
        contract_start = profile.created_date + timedelta(days=self.rng.randint(0, first_window))
        contract_start = max(contract_start, self.simulation_start)
        signed_date_override: date | None = None
        linked_previous_id = ""
        history: list[dict[str, Any]] = []
        relationship_state = profile.relationship_baseline

        for sequence_number in range(1, self.config.max_contracts_per_customer + 1):
            if contract_start > self.simulation_end:
                break

            contract_id = self._next_contract_id()
            if linked_previous_id:
                previous_row = self._contract_rows_by_id[linked_previous_id]
                previous_row["successor_contract_id"] = contract_id

            previous_linked = history[-1] if linked_previous_id and history else None
            contract = self._create_contract(
                profile=profile,
                contract_id=contract_id,
                sequence_number=sequence_number,
                start_date=contract_start,
                signed_date_override=signed_date_override,
                previous_contract_id=linked_previous_id,
                previous_linked=previous_linked,
            )
            cutoff = contract["end_date"] - timedelta(days=self.config.prediction_lead_days)
            round_events, relationship_at_cutoff, final_relationship = self._simulate_rounds(
                profile=profile,
                contract=contract,
                cutoff=cutoff,
                initial_relationship=relationship_state,
            )

            snapshot = self._build_snapshot(
                profile=profile,
                contract=contract,
                current_rounds=round_events,
                history=history,
                cutoff=cutoff,
            )

            label_window_end = contract["end_date"] + timedelta(
                days=self.config.label_observation_days
            )
            label_observed = label_window_end <= self.simulation_end
            outcome = "pending"
            renewal_label: int | str = ""
            decision_date: date | None = None
            next_start: date | None = None
            next_signed: date | None = None

            if label_observed:
                outcome, decision_date, next_start = self._choose_customer_outcome(
                    profile=profile,
                    contract=contract,
                    history=history,
                    snapshot=snapshot,
                    relationship_at_cutoff=relationship_at_cutoff,
                    final_relationship=final_relationship,
                )
                renewal_label = 1 if outcome == "renewed" else 0
                if outcome == "renewed":
                    next_signed = decision_date

            contract_row = self._contract_to_row(
                contract=contract,
                outcome=outcome,
                decision_date=decision_date,
                label_observed=label_observed,
            )
            self.contracts.append(contract_row)
            self._contract_rows_by_id[contract_id] = contract_row
            self.monitoring_rounds.extend(self._rounds_to_rows(round_events))

            snapshot["label_observed"] = int(label_observed)
            snapshot["label_window_end"] = iso(label_window_end)
            snapshot[LABEL_COLUMN] = renewal_label
            snapshot["split"] = "inference_only" if not label_observed else ""
            self.snapshots.append(snapshot)

            history.append(
                {
                    "contract": contract,
                    "rounds": round_events,
                    "renewal_outcome": renewal_label,
                    "label_observed": label_observed,
                    "relationship_at_cutoff": relationship_at_cutoff,
                }
            )
            relationship_state = clamp(
                0.82 * final_relationship
                + 0.18 * profile.relationship_baseline
                + self.rng.gauss(0.0, profile.decision_volatility / 4),
                0.02,
                0.98,
            )

            if not label_observed:
                break
            if outcome == "renewed" and next_start is not None:
                contract_start = next_start
                signed_date_override = next_signed
                linked_previous_id = contract_id
                continue

            return_probability = clamp(
                0.04
                + 0.18 * profile.compliance_need
                + 0.08 * profile.business_health
                - 0.10 * profile.price_sensitivity,
                0.02,
                0.28,
            )
            if self.rng.random() >= return_probability:
                break
            gap_days = self.rng.randint(180, 720)
            contract_start = contract["end_date"] + timedelta(days=gap_days)
            signed_date_override = contract_start - timedelta(days=self.rng.randint(14, 75))
            linked_previous_id = ""
            relationship_state = clamp(
                0.55 * relationship_state + 0.45 * profile.relationship_baseline,
                0.02,
                0.98,
            )

    def _create_contract(
        self,
        profile: CustomerProfile,
        contract_id: str,
        sequence_number: int,
        start_date: date,
        signed_date_override: date | None,
        previous_contract_id: str,
        previous_linked: dict[str, Any] | None,
    ) -> dict[str, Any]:
        duration = weighted_choice(
            self.rng,
            [(6, 0.12), (12, 0.52), (18, 0.16), (24, 0.17), (36, 0.03)],
        )
        if previous_linked and self.rng.random() < 0.72:
            previous_duration = previous_linked["contract"]["duration_months"]
            duration = weighted_choice(
                self.rng,
                [(previous_duration, 0.72), (12, 0.18), (24, 0.10)],
            )

        end_date = add_months(start_date, duration) - timedelta(days=1)
        if signed_date_override:
            signed_date = min(signed_date_override, start_date)
        else:
            signed_date = start_date - timedelta(days=self.rng.randint(10, 75))

        if previous_linked and self.rng.random() < 0.78:
            frequency_code = previous_linked["contract"]["frequency_code"]
        else:
            frequency_code = weighted_choice(
                self.rng,
                [("TSQT01", 0.14), ("TSQT02", 0.29), ("TSQT03", 0.57)],
            )
        if duration == 6 and frequency_code == "TSQT02":
            frequency_code = weighted_choice(self.rng, [("TSQT01", 0.45), ("TSQT03", 0.55)])

        sector_complexity = SECTORS[profile.sector]["complexity"]
        if previous_linked:
            previous_complexity = previous_linked["contract"]["complexity"]
            complexity = clamp(
                0.72 * previous_complexity
                + 0.28 * sector_complexity
                + self.rng.gauss(0, 0.08),
                0.08,
                0.98,
            )
        else:
            complexity = clamp(
                0.62 * sector_complexity
                + 0.38 * self.rng.betavariate(2.5, 2.3),
                0.08,
                0.98,
            )

        size_budget = COMPANY_SIZES[profile.company_size]["budget"]
        frequency_multiplier = {"TSQT01": 0.58, "TSQT02": 0.84, "TSQT03": 1.20}[
            frequency_code
        ]
        base_value = 72.0 * (duration / 12.0) * frequency_multiplier * size_budget
        base_value *= 0.72 + 0.85 * complexity
        if previous_linked:
            prior_value = previous_linked["contract"]["contract_value_million_vnd"]
            inflation_and_scope = self.rng.gauss(0.055, 0.075)
            contract_value = max(base_value * 0.55, prior_value * (1 + inflation_and_scope))
        else:
            contract_value = base_value * self.rng.lognormvariate(0.0, 0.22)

        return {
            "contract_id": contract_id,
            "customer_id": profile.customer_id,
            "sequence_number": sequence_number,
            "previous_contract_id": previous_contract_id,
            "signed_date": signed_date,
            "start_date": start_date,
            "end_date": end_date,
            "duration_months": duration,
            "frequency_code": frequency_code,
            "complexity": round(complexity, 4),
            "contract_value_million_vnd": round(contract_value, 2),
        }

    def _scheduled_due_dates(self, contract: dict[str, Any]) -> list[date]:
        start = contract["start_date"]
        end = contract["end_date"]
        frequency_months = FREQUENCY_MONTHS[contract["frequency_code"]]
        if frequency_months is None:
            span = (end - start).days
            due = start + timedelta(days=max(30, int(span * self.rng.uniform(0.42, 0.70))))
            return [min(due, end)]

        due_dates: list[date] = []
        offset = frequency_months
        while True:
            due = add_months(start, offset) - timedelta(days=1)
            if due > end:
                break
            due_dates.append(due)
            offset += frequency_months
        if not due_dates or (end - due_dates[-1]).days > frequency_months * 31 // 2:
            due_dates.append(end)
        return sorted(set(due_dates))

    def _simulate_rounds(
        self,
        profile: CustomerProfile,
        contract: dict[str, Any],
        cutoff: date,
        initial_relationship: float,
    ) -> tuple[list[dict[str, Any]], float, float]:
        events: list[dict[str, Any]] = []
        relationship_state = initial_relationship
        relationship_at_cutoff = initial_relationship
        operational_momentum = self.rng.gauss(0.0, 0.20)

        for round_number, due_date in enumerate(self._scheduled_due_dates(contract), start=1):
            self._round_counter += 1
            complexity = contract["complexity"]
            seasonal_load = 0.18 if due_date.month in (3, 6, 9, 12) else 0.0
            operational_momentum = clamp(
                0.66 * operational_momentum + self.rng.gauss(0.0, 0.22),
                -0.75,
                0.85,
            )
            weather_shock = self.rng.random() < (0.05 + 0.08 * complexity)
            lab_shock = self.rng.random() < (0.035 + 0.055 * complexity)
            customer_reschedule = self.rng.random() < (
                0.06 + 0.12 * (1 - profile.coordination_quality)
            )
            major_disruption = self.rng.random() < (
                0.018
                + 0.045 * complexity
                + 0.025 * max(0.0, operational_momentum)
            )
            shock_days = (
                (self.rng.randint(2, 9) if weather_shock else 0)
                + (self.rng.randint(2, 7) if lab_shock else 0)
                + (self.rng.randint(2, 8) if customer_reschedule else 0)
                + (self.rng.randint(25, 75) if major_disruption else 0)
            )

            planned_processing_days = int(round(12 + 15 * complexity + self.rng.uniform(-3, 4)))
            planned_start = due_date - timedelta(days=max(7, planned_processing_days))
            start_delay_mean = (
                -0.5
                + 2.8 * complexity
                + 2.2 * seasonal_load
                + 2.0 * max(0.0, operational_momentum)
                - 3.2 * profile.coordination_quality
            )
            start_delay = max(0, int(round(self.rng.gauss(start_delay_mean, 3.0))))
            actual_start = planned_start + timedelta(days=start_delay)

            incident_rate = (
                0.05
                + 0.20 * complexity
                + 0.10 * seasonal_load
                + 0.10 * max(0.0, operational_momentum)
            )
            incident_count = poisson(self.rng, incident_rate, cap=3)
            actual_processing = int(
                round(
                    planned_processing_days
                    + self.rng.gauss(-5.0 + 1.6 * complexity, 3.6)
                    + shock_days
                    + 3.0 * incident_count
                )
            )
            actual_processing = max(4, actual_processing)

            revision_lambda = (
                0.10
                + 0.55 * complexity
                + 0.20 * incident_count
                + 0.18 * max(0.0, operational_momentum)
            )
            revision_count = poisson(self.rng, revision_lambda, cap=4)
            revision_days = sum(self.rng.randint(1, 4) for _ in range(revision_count))
            result_date = actual_start + timedelta(days=actual_processing + revision_days)
            delay_days = (result_date - due_date).days

            complaint_probability = clamp(
                0.015
                + 0.012 * max(delay_days, 0)
                + 0.07 * revision_count
                + 0.10 * incident_count
                + 0.05 * profile.decision_volatility,
                0.01,
                0.72,
            )
            complaint_count = 1 if self.rng.random() < complaint_probability else 0
            if complaint_count and self.rng.random() < 0.10:
                complaint_count += 1

            observed_quality = clamp(
                0.92
                - 0.018 * max(delay_days, 0)
                - 0.10 * revision_count
                - 0.13 * incident_count
                - 0.16 * complaint_count
                + self.rng.gauss(0.0, 0.11),
                0.0,
                1.0,
            )
            relationship_state = clamp(
                0.52 * relationship_state
                + 0.40 * observed_quality
                + 0.08 * profile.relationship_baseline
                + self.rng.gauss(0.0, profile.decision_volatility / 5),
                0.01,
                0.99,
            )

            feedback_score: float | None = None
            feedback_probability = clamp(
                0.16
                + 0.55 * profile.feedback_propensity
                + 0.10 * complaint_count,
                0.08,
                0.88,
            )
            if self.rng.random() < feedback_probability:
                feedback_score = round(
                    clamp(1.0 + 4.0 * relationship_state + self.rng.gauss(0, 0.45), 1.0, 5.0),
                    1,
                )

            event = {
                "round_id": f"DQT_SYN_{self._round_counter:07d}",
                "customer_id": profile.customer_id,
                "contract_id": contract["contract_id"],
                "round_number": round_number,
                "planned_start_date": planned_start,
                "actual_start_date": actual_start,
                "planned_result_date": due_date,
                "actual_result_date": result_date,
                "delay_days": delay_days,
                "processing_days": (result_date - actual_start).days,
                "report_revision_count": revision_count,
                "incident_count": incident_count,
                "complaint_count": complaint_count,
                "feedback_score": feedback_score,
                "weather_disruption": int(weather_shock),
                "customer_rescheduled": int(customer_reschedule),
                "major_disruption": int(major_disruption),
                "is_synthetic": 1,
            }
            events.append(event)
            if result_date <= cutoff:
                relationship_at_cutoff = relationship_state

        return events, relationship_at_cutoff, relationship_state

    def _build_snapshot(
        self,
        profile: CustomerProfile,
        contract: dict[str, Any],
        current_rounds: list[dict[str, Any]],
        history: list[dict[str, Any]],
        cutoff: date,
    ) -> dict[str, Any]:
        due = [event for event in current_rounds if event["planned_result_date"] <= cutoff]
        completed = [event for event in current_rounds if event["actual_result_date"] <= cutoff]
        overdue_open = [
            event
            for event in due
            if event["actual_result_date"] > cutoff
        ]
        completed_delays = [max(0, event["delay_days"]) for event in completed]
        completed_processing = [event["processing_days"] for event in completed]
        recent_start = cutoff - timedelta(days=90)
        recent = [
            event
            for event in completed
            if recent_start <= event["actual_result_date"] <= cutoff
        ]
        recent_delays = [max(0, event["delay_days"]) for event in recent]

        historical_rounds: list[dict[str, Any]] = []
        for item in history:
            historical_rounds.extend(
                event
                for event in item["rounds"]
                if event["actual_result_date"] <= cutoff
            )
        historical_delays = [max(0, event["delay_days"]) for event in historical_rounds]
        historical_processing = [event["processing_days"] for event in historical_rounds]
        historical_outcomes = [
            int(item["renewal_outcome"])
            for item in history
            if item["label_observed"] and item["renewal_outcome"] != ""
        ]

        previous_contract_count = len(history)
        first_contract_start = (
            history[0]["contract"]["start_date"] if history else contract["start_date"]
        )
        previous_end = history[-1]["contract"]["end_date"] if history else None
        latest_event = max(
            (event["actual_result_date"] for event in completed),
            default=None,
        )

        return {
            "snapshot_id": f"SNAP_{contract['contract_id']}",
            "customer_id": profile.customer_id,
            "contract_id": contract["contract_id"],
            "snapshot_date": iso(cutoff),
            "contract_end_date": iso(contract["end_date"]),
            "latest_observed_event_date": iso(latest_event),
            "label_window_end": "",
            "label_observed": 0,
            "split": "",
            "frequency_code": contract["frequency_code"],
            "contract_sequence_number": contract["sequence_number"],
            "contract_duration_months": contract["duration_months"],
            "days_observed_current_contract": max(0, (cutoff - contract["start_date"]).days),
            "expected_rounds_total": len(current_rounds),
            "rounds_due_by_cutoff": len(due),
            "rounds_completed_by_cutoff": len(completed),
            "open_overdue_rounds_at_cutoff": len(overdue_open),
            "completion_rate_to_cutoff": ratio(len(completed), len(due)),
            "on_time_rate_completed": ratio(
                sum(event["delay_days"] <= 0 for event in completed),
                len(completed),
            ),
            "average_delay_days": mean_or_none(completed_delays),
            "maximum_delay_days": max(completed_delays) if completed_delays else None,
            "average_processing_days": mean_or_none(completed_processing),
            "maximum_processing_days": max(completed_processing) if completed_processing else None,
            "recent_90d_completed_rounds": len(recent),
            "recent_90d_average_delay_days": mean_or_none(recent_delays),
            "has_customer_history": int(previous_contract_count > 0),
            "previous_contract_count": previous_contract_count,
            "relationship_tenure_days": max(0, (cutoff - first_contract_start).days),
            "days_since_previous_contract_end": (
                max(0, (contract["start_date"] - previous_end).days)
                if previous_end
                else None
            ),
            "historical_rounds_completed": len(historical_rounds),
            "historical_completion_rate": self._historical_completion_rate(history, cutoff),
            "historical_on_time_rate": ratio(
                sum(event["delay_days"] <= 0 for event in historical_rounds),
                len(historical_rounds),
            ) if historical_rounds else None,
            "historical_average_delay_days": mean_or_none(historical_delays),
            "historical_average_processing_days": mean_or_none(historical_processing),
            "historical_renewal_rate": mean_or_none(historical_outcomes),
            "current_metrics_available": int(bool(completed)),
            "historical_metrics_available": int(bool(historical_rounds)),
            LABEL_COLUMN: "",
        }

    def _historical_completion_rate(
        self,
        history: list[dict[str, Any]],
        cutoff: date,
    ) -> float | None:
        if not history:
            return None
        total_due = 0
        total_completed = 0
        for item in history:
            for event in item["rounds"]:
                if event["planned_result_date"] <= cutoff:
                    total_due += 1
                if event["actual_result_date"] <= cutoff:
                    total_completed += 1
        return ratio(total_completed, total_due) if total_due else None

    def _choose_customer_outcome(
        self,
        profile: CustomerProfile,
        contract: dict[str, Any],
        history: list[dict[str, Any]],
        snapshot: dict[str, Any],
        relationship_at_cutoff: float,
        final_relationship: float,
    ) -> tuple[str, date | None, date | None]:
        # These hidden shocks are intentionally not exported as model features.
        competitor_attractiveness = self.rng.betavariate(2.4, 2.7)
        regulation_shock = self.rng.gauss(0.0, 0.08)
        commercial_shock = self.rng.gauss(
            0.0, 0.10 + 0.20 * profile.decision_volatility
        )
        future_need = clamp(
            0.68 * profile.compliance_need
            + 0.20 * profile.business_health
            + 0.12 * self.rng.random()
            + regulation_shock,
            0.01,
            0.99,
        )
        proposed_price_change = self.rng.gauss(0.055, 0.095)
        history_depth = min(1.0, len(history) / 4.0)
        service_delivery_score = self._service_delivery_score(snapshot)
        previous_renewal_rate = snapshot["historical_renewal_rate"]
        renewal_habit = (
            float(previous_renewal_rate)
            if previous_renewal_rate is not None
            else 0.5
        )
        # Service delivered by T-60 should matter materially, while late events,
        # commercial pressure and chance can still reverse an otherwise likely
        # decision. This keeps the task noisy without making observable operations
        # irrelevant to the label.
        decision_relationship = (
            0.95 * relationship_at_cutoff + 0.05 * final_relationship
        )

        noise_scale = 0.25 + 0.25 * profile.decision_volatility
        renew_utility = (
            -2.45
            + 1.18 * future_need
            + 2.85 * decision_relationship
            + 0.55 * profile.loyalty
            + 0.24 * history_depth
            + 3.35 * (service_delivery_score - 0.65)
            + 0.85 * (renewal_habit - 0.5)
            - 0.90 * profile.price_sensitivity * max(proposed_price_change, 0.0)
            - 0.45 * competitor_attractiveness
            + commercial_shock
            + noise_scale * gumbel(self.rng)
        )
        switch_utility = (
            -0.18
            + 1.02 * future_need
            + 0.55 * competitor_attractiveness
            + 0.38 * profile.price_sensitivity
            - 1.35 * decision_relationship
            + 1.90 * (0.65 - service_delivery_score)
            + noise_scale * gumbel(self.rng)
        )
        pause_utility = (
            -0.08
            + 1.22 * (1 - future_need)
            + 0.72 * (1 - profile.business_health)
            + noise_scale * gumbel(self.rng)
        )

        outcome = max(
            (
                ("renewed", renew_utility),
                ("switched_provider", switch_utility),
                ("paused_service", pause_utility),
            ),
            key=lambda item: item[1],
        )[0]
        if outcome != "renewed":
            return outcome, None, None

        cutoff = contract["end_date"] - timedelta(days=self.config.prediction_lead_days)
        if self.rng.random() < 0.84:
            decision_date = cutoff + timedelta(
                days=self.rng.randint(7, max(8, self.config.prediction_lead_days - 3))
            )
        else:
            decision_date = contract["end_date"] + timedelta(days=self.rng.randint(1, 55))
        latest_allowed = contract["end_date"] + timedelta(
            days=self.config.label_observation_days - 10
        )
        decision_date = min(decision_date, latest_allowed)
        nominal_start = contract["end_date"] + timedelta(days=self.rng.randint(1, 24))
        next_start = max(nominal_start, decision_date + timedelta(days=self.rng.randint(0, 10)))
        next_start = min(
            next_start,
            contract["end_date"] + timedelta(days=self.config.label_observation_days),
        )
        return outcome, decision_date, next_start

    @staticmethod
    def _service_delivery_score(snapshot: dict[str, Any]) -> float:
        """Summarise customer-visible delivery evidence available at T-60.

        The simulator uses this as one input to a noisy three-way commercial
        decision. It is deliberately not exported as a model feature: the model
        must learn from the underlying completion, timeliness and delay measures.
        """

        completed = int(snapshot["rounds_completed_by_cutoff"])
        due = int(snapshot["rounds_due_by_cutoff"])
        completion = (
            float(snapshot["completion_rate_to_cutoff"])
            if due > 0
            else 0.62
        )
        on_time = (
            float(snapshot["on_time_rate_completed"])
            if completed > 0
            else 0.50
        )
        average_delay = float(snapshot["average_delay_days"] or 0.0)
        recent_delay = snapshot["recent_90d_average_delay_days"]
        delay_score = math.exp(-average_delay / 8.0)
        recent_score = (
            math.exp(-float(recent_delay) / 8.0)
            if recent_delay is not None
            else delay_score
        )
        historical_on_time = snapshot["historical_on_time_rate"]
        historical_completion = snapshot["historical_completion_rate"]
        history_score = (
            0.55 * float(historical_on_time)
            + 0.45 * float(historical_completion)
            if historical_on_time is not None and historical_completion is not None
            else 0.55 * on_time + 0.45 * completion
        )
        overdue_ratio = min(
            1.0,
            int(snapshot["open_overdue_rounds_at_cutoff"]) / max(1, due),
        )
        return clamp(
            0.27 * completion
            + 0.25 * on_time
            + 0.20 * delay_score
            + 0.13 * recent_score
            + 0.15 * history_score
            - 0.30 * overdue_ratio,
            0.0,
            1.0,
        )

    def _contract_to_row(
        self,
        contract: dict[str, Any],
        outcome: str,
        decision_date: date | None,
        label_observed: bool,
    ) -> dict[str, Any]:
        return {
            "contract_id": contract["contract_id"],
            "customer_id": contract["customer_id"],
            "contract_number": f"HD-SYN-{contract['contract_id'].split('_')[-1]}",
            "contract_sequence_number": contract["sequence_number"],
            "previous_contract_id": contract["previous_contract_id"],
            "successor_contract_id": "",
            "signed_date": iso(contract["signed_date"]),
            "start_date": iso(contract["start_date"]),
            "end_date": iso(contract["end_date"]),
            "duration_months": contract["duration_months"],
            "frequency_code": contract["frequency_code"],
            "contract_value_million_vnd": contract["contract_value_million_vnd"],
            "renewal_decision_date": iso(decision_date),
            "renewal_outcome": outcome,
            "label_observed": int(label_observed),
            "is_synthetic": 1,
        }

    def _rounds_to_rows(self, events: Iterable[dict[str, Any]]) -> list[dict[str, Any]]:
        rows: list[dict[str, Any]] = []
        for event in events:
            row = dict(event)
            for key in (
                "planned_start_date",
                "actual_start_date",
                "planned_result_date",
                "actual_result_date",
            ):
                row[key] = iso(row[key])
            rows.append(row)
        return rows

    def _assign_time_splits(self) -> None:
        labelled = sorted(
            (row for row in self.snapshots if row["label_observed"] == 1),
            key=lambda row: (row["snapshot_date"], row["contract_id"]),
        )
        if not labelled:
            raise ValueError("No labelled snapshots were generated")
        train_index = max(0, int(len(labelled) * 0.70) - 1)
        validation_index = max(train_index, int(len(labelled) * 0.85) - 1)
        train_cutoff = labelled[train_index]["snapshot_date"]
        validation_cutoff = labelled[validation_index]["snapshot_date"]
        for row in labelled:
            if row["snapshot_date"] <= train_cutoff:
                row["split"] = "train"
            elif row["snapshot_date"] <= validation_cutoff:
                row["split"] = "validation"
            else:
                row["split"] = "test"

    def _validate_in_memory(self) -> None:
        if len({row["customer_id"] for row in self.customers}) != len(self.customers):
            raise ValueError("Duplicate customer_id detected")
        if len({row["contract_id"] for row in self.contracts}) != len(self.contracts):
            raise ValueError("Duplicate contract_id detected")
        if len({row["round_id"] for row in self.monitoring_rounds}) != len(self.monitoring_rounds):
            raise ValueError("Duplicate round_id detected")
        if len({row["contract_id"] for row in self.snapshots}) != len(self.snapshots):
            raise ValueError("More than one snapshot was generated for a contract")

        contracts_by_id = {row["contract_id"]: row for row in self.contracts}
        for row in self.monitoring_rounds:
            if row["contract_id"] not in contracts_by_id:
                raise ValueError(f"Orphan monitoring round: {row['round_id']}")
        for row in self.contracts:
            if row["renewal_outcome"] != "renewed":
                continue
            successor_id = row["successor_contract_id"]
            if not successor_id:
                raise ValueError(
                    f"Renewed contract {row['contract_id']} has no successor. "
                    "Increase max_contracts_per_customer so the simulated chain is not truncated."
                )
            successor = contracts_by_id.get(successor_id)
            if successor is None or successor["previous_contract_id"] != row["contract_id"]:
                raise ValueError(f"Broken successor link for {row['contract_id']}")
        for row in self.snapshots:
            expected = parse_date(row["contract_end_date"]) - timedelta(
                days=self.config.prediction_lead_days
            )
            if parse_date(row["snapshot_date"]) != expected:
                raise ValueError(f"Invalid T-60 cutoff for {row['contract_id']}")
            latest = row["latest_observed_event_date"]
            if latest and parse_date(latest) > parse_date(row["snapshot_date"]):
                raise ValueError(f"Feature leakage detected for {row['contract_id']}")

        labelled = [row for row in self.snapshots if row["label_observed"] == 1]
        labels = [int(row[LABEL_COLUMN]) for row in labelled]
        renewal_rate = statistics.fmean(labels)
        if not 0.15 <= renewal_rate <= 0.85:
            raise ValueError(
                f"Generated renewal rate {renewal_rate:.3f} is implausibly extreme; "
                "adjust the stochastic decision process"
            )
        if not any(row["has_customer_history"] == 0 for row in labelled):
            raise ValueError("No cold-start examples were generated")
        if not all(row["split"] in {"train", "validation", "test"} for row in labelled):
            raise ValueError("A labelled row has no time split")

    def _next_contract_id(self) -> str:
        self._contract_counter += 1
        return f"HD_SYN_{self._contract_counter:07d}"

    def training_rows(self) -> list[dict[str, Any]]:
        columns = SNAPSHOT_METADATA_COLUMNS + MODEL_FEATURES + [LABEL_COLUMN]
        return [
            {column: row.get(column, "") for column in columns}
            for row in self.snapshots
            if row["label_observed"] == 1
        ]

    def write_outputs(self, output_directory: Path) -> dict[str, Any]:
        output_directory.mkdir(parents=True, exist_ok=True)
        training = self.training_rows()
        files_and_rows: list[tuple[str, list[dict[str, Any]]]] = [
            ("customers.csv", self.customers),
            ("contracts.csv", self.contracts),
            ("monitoring_rounds.csv", self.monitoring_rounds),
            ("prediction_snapshots.csv", self.snapshots),
            ("training_dataset.csv", training),
        ]
        if self.config.include_latent_debug:
            files_and_rows.append(("latent_debug_DO_NOT_TRAIN.csv", self.latent_debug))

        written_paths: list[Path] = []
        for filename, rows in files_and_rows:
            path = output_directory / filename
            self._write_csv(path, rows)
            written_paths.append(path)

        feature_spec = {
            "categorical_features": MODEL_FEATURES_CATEGORICAL,
            "numeric_features": MODEL_FEATURES_NUMERIC,
            "label": LABEL_COLUMN,
            "metadata_not_for_training": SNAPSHOT_METADATA_COLUMNS,
            "explicitly_excluded_as_leakage": [
                "successor_contract_id",
                "renewal_decision_date",
                "renewal_outcome",
                "events_after_snapshot_date",
                "latent_debug_DO_NOT_TRAIN.csv",
            ],
        }
        feature_path = output_directory / "feature_columns.json"
        self._write_json(feature_path, feature_spec)
        written_paths.append(feature_path)

        report = self._build_report(training)
        report_path = output_directory / "generation_report.json"
        self._write_json(report_path, report)
        written_paths.append(report_path)

        manifest = {
            "generator": "ECOS event-based synthetic renewal generator",
            "generated_at_utc": datetime.now(timezone.utc).isoformat(),
            "seed": self.config.seed,
            "config": asdict(self.config),
            "files": {
                path.name: {
                    "sha256": hashlib.sha256(path.read_bytes()).hexdigest(),
                    "bytes": path.stat().st_size,
                }
                for path in written_paths
            },
        }
        self._write_json(output_directory / "manifest.json", manifest)
        return report

    def _build_report(self, training: list[dict[str, Any]]) -> dict[str, Any]:
        labels = [int(row[LABEL_COLUMN]) for row in training]
        split_counts = Counter(row["split"] for row in training)
        frequency_counts = Counter(row["frequency_code"] for row in training)
        duration_counts = Counter(str(row["contract_duration_months"]) for row in training)
        cold_start_count = sum(row["has_customer_history"] == 0 for row in training)
        current_missing_count = sum(row["current_metrics_available"] == 0 for row in training)
        average_delays = [
            float(row["average_delay_days"])
            for row in training
            if row["average_delay_days"] not in (None, "")
        ]
        on_time_rates = [
            float(row["on_time_rate_completed"])
            for row in training
            if row["on_time_rate_completed"] not in (None, "")
        ]
        processing_days = [
            float(row["average_processing_days"])
            for row in training
            if row["average_processing_days"] not in (None, "")
        ]

        def quartiles(values: list[float]) -> list[float]:
            if len(values) < 2:
                return []
            return [
                round(value, 4)
                for value in statistics.quantiles(values, n=4, method="inclusive")
            ]

        return {
            "data_origin": "fully synthetic; no enterprise records were used",
            "generation_method": (
                "customer/contract/monitoring events first; one T-60 snapshot per contract; "
                "stochastic hidden renewal decision with future and unobserved shocks"
            ),
            "seed": self.config.seed,
            "customer_count": len(self.customers),
            "contract_count": len(self.contracts),
            "monitoring_round_count": len(self.monitoring_rounds),
            "all_snapshot_count": len(self.snapshots),
            "labelled_training_row_count": len(training),
            "renewal_rate": round(statistics.fmean(labels), 4),
            "cold_start_rate": round(ratio(cold_start_count, len(training)), 4),
            "current_metrics_missing_rate": round(
                ratio(current_missing_count, len(training)), 4
            ),
            "split_counts": dict(sorted(split_counts.items())),
            "frequency_counts": dict(sorted(frequency_counts.items())),
            "duration_month_counts": dict(sorted(duration_counts.items())),
            "renewal_rate_by_split": {
                split: round(
                    statistics.fmean(
                        int(row[LABEL_COLUMN])
                        for row in training
                        if row["split"] == split
                    ),
                    4,
                )
                for split in ("train", "validation", "test")
            },
            "observable_distribution_summary": {
                "average_delay_days_quartiles": quartiles(average_delays),
                "on_time_rate_quartiles": quartiles(on_time_rates),
                "average_processing_days_quartiles": quartiles(processing_days),
                "contracts_with_open_overdue_round_at_cutoff_rate": round(
                    ratio(
                        sum(
                            int(row["open_overdue_rounds_at_cutoff"]) > 0
                            for row in training
                        ),
                        len(training),
                    ),
                    4,
                ),
            },
            "sanity_checks": {
                "one_snapshot_per_contract": True,
                "all_snapshot_features_use_events_on_or_before_cutoff": True,
                "all_renewed_contracts_have_linked_successor": True,
                "duplicate_training_contract_count": (
                    len(training) - len({row["contract_id"] for row in training})
                ),
                "renewed_despite_average_delay_at_least_5_days_count": sum(
                    int(row[LABEL_COLUMN]) == 1
                    and float(row["average_delay_days"] or 0) >= 5
                    for row in training
                ),
                "not_renewed_despite_average_delay_at_most_1_day_count": sum(
                    int(row[LABEL_COLUMN]) == 0
                    and float(row["average_delay_days"] or 0) <= 1
                    for row in training
                ),
            },
            "prediction_lead_days": self.config.prediction_lead_days,
            "label_observation_days_after_contract_end": self.config.label_observation_days,
            "model_feature_count": len(MODEL_FEATURES),
            "model_features": MODEL_FEATURES,
            "important_limitations": [
                "Metrics estimate performance against the simulator, not real customers.",
                "Latent customer traits and post-cutoff events affect outcomes but are not model inputs.",
                "The default model feature set is restricted to data derivable from the existing ECOS contract and monitoring-round schema.",
                "T-60 and the T+90 observation window are business assumptions and have not been validated with an enterprise dataset.",
            ],
        }

    @staticmethod
    def _write_csv(path: Path, rows: list[dict[str, Any]]) -> None:
        if not rows:
            raise ValueError(f"Refusing to write empty dataset: {path.name}")
        fieldnames = list(rows[0].keys())
        with path.open("w", encoding="utf-8-sig", newline="") as handle:
            writer = csv.DictWriter(handle, fieldnames=fieldnames)
            writer.writeheader()
            writer.writerows(rows)

    @staticmethod
    def _write_json(path: Path, payload: dict[str, Any]) -> None:
        with path.open("w", encoding="utf-8") as handle:
            json.dump(payload, handle, ensure_ascii=False, indent=2, sort_keys=True)
            handle.write("\n")


def resolve_output_directory(config_path: Path, config: GeneratorConfig, override: str | None) -> Path:
    if override:
        path = Path(override)
        return path if path.is_absolute() else (Path.cwd() / path).resolve()
    configured = Path(config.output_directory)
    return configured if configured.is_absolute() else (config_path.parent / configured).resolve()


def build_argument_parser() -> argparse.ArgumentParser:
    parser = argparse.ArgumentParser(
        description="Generate reproducible, event-based synthetic renewal data for ECOS."
    )
    parser.add_argument(
        "--config",
        default=str(SCRIPT_DIR / "config.json"),
        help="Path to the generator JSON config.",
    )
    parser.add_argument(
        "--output",
        default=None,
        help="Optional output directory override.",
    )
    return parser


def main() -> int:
    args = build_argument_parser().parse_args()
    config_path = Path(args.config).resolve()
    config = GeneratorConfig.from_json(config_path)
    generator = SyntheticRenewalGenerator(config)
    generator.generate()
    output_directory = resolve_output_directory(config_path, config, args.output)
    report = generator.write_outputs(output_directory)
    print(json.dumps(report, ensure_ascii=False, indent=2, sort_keys=True))
    print(f"\nGenerated files: {output_directory}")
    return 0


if __name__ == "__main__":
    raise SystemExit(main())
