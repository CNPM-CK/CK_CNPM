import csv
import json
import statistics
import tempfile
import unittest
from dataclasses import replace
from datetime import date, timedelta
from pathlib import Path
import sys


MODULE_DIR = Path(__file__).resolve().parents[1]
sys.path.insert(0, str(MODULE_DIR))

from generate import GeneratorConfig, SyntheticRenewalGenerator  # noqa: E402


class SyntheticRenewalGeneratorTests(unittest.TestCase):
    def make_config(self, seed: int = 31012026) -> GeneratorConfig:
        return GeneratorConfig(
            seed=seed,
            customer_count=80,
            simulation_start="2017-01-01",
            simulation_end="2025-12-31",
            prediction_lead_days=60,
            label_observation_days=90,
            max_contracts_per_customer=20,
            include_latent_debug=False,
            output_directory="output",
        )

    def test_business_keys_cutoff_and_leakage_invariants(self) -> None:
        config = self.make_config()
        generator = SyntheticRenewalGenerator(config)
        generator.generate()

        customer_ids = {row["customer_id"] for row in generator.customers}
        contract_ids = {row["contract_id"] for row in generator.contracts}
        self.assertEqual(len(customer_ids), len(generator.customers))
        self.assertEqual(len(contract_ids), len(generator.contracts))
        self.assertTrue(generator.training_rows())

        snapshot_contracts = set()
        for snapshot in generator.snapshots:
            self.assertNotIn(snapshot["contract_id"], snapshot_contracts)
            snapshot_contracts.add(snapshot["contract_id"])
            expected = date.fromisoformat(snapshot["contract_end_date"]) - timedelta(days=60)
            self.assertEqual(date.fromisoformat(snapshot["snapshot_date"]), expected)
            if snapshot["latest_observed_event_date"]:
                self.assertLessEqual(
                    date.fromisoformat(snapshot["latest_observed_event_date"]),
                    date.fromisoformat(snapshot["snapshot_date"]),
                )

        for monitoring_round in generator.monitoring_rounds:
            self.assertIn(monitoring_round["contract_id"], contract_ids)

        contracts_by_id = {row["contract_id"]: row for row in generator.contracts}
        for contract in generator.contracts:
            if contract["renewal_outcome"] == "renewed":
                successor_id = contract["successor_contract_id"]
                self.assertIn(successor_id, contracts_by_id)
                self.assertEqual(
                    contracts_by_id[successor_id]["previous_contract_id"],
                    contract["contract_id"],
                )

    def test_cold_start_and_non_deterministic_edge_cases_exist(self) -> None:
        generator = SyntheticRenewalGenerator(self.make_config())
        generator.generate()
        training = generator.training_rows()
        self.assertTrue(any(row["has_customer_history"] == 0 for row in training))
        self.assertTrue(any(row["has_customer_history"] == 1 for row in training))
        self.assertEqual({0, 1}, {int(row["renewed_with_new_contract"]) for row in training})

        # At least one outcome should violate the naive rule "no delay => renew" or
        # "delay => do not renew", proving labels are not a direct threshold.
        surprising_positive = any(
            int(row["renewed_with_new_contract"]) == 1
            and (row["average_delay_days"] or 0) >= 5
            for row in training
        )
        surprising_negative = any(
            int(row["renewed_with_new_contract"]) == 0
            and (row["average_delay_days"] or 0) <= 1
            for row in training
        )
        self.assertTrue(surprising_positive or surprising_negative)

    def test_same_seed_produces_identical_training_csv(self) -> None:
        config = self.make_config(seed=777)
        first = SyntheticRenewalGenerator(config)
        second = SyntheticRenewalGenerator(config)
        first.generate()
        second.generate()

        with tempfile.TemporaryDirectory() as first_dir, tempfile.TemporaryDirectory() as second_dir:
            first.write_outputs(Path(first_dir))
            second.write_outputs(Path(second_dir))
            self.assertEqual(
                (Path(first_dir) / "training_dataset.csv").read_bytes(),
                (Path(second_dir) / "training_dataset.csv").read_bytes(),
            )

    def test_observable_service_quality_has_signal_but_not_perfect_labels(self) -> None:
        config = replace(self.make_config(seed=20260819), customer_count=600)
        generator = SyntheticRenewalGenerator(config)
        generator.generate()
        training = generator.training_rows()
        low_delay_labels = [
            int(row["renewed_with_new_contract"])
            for row in training
            if row["average_delay_days"] is not None
            and float(row["average_delay_days"]) <= 1
        ]
        high_delay_labels = [
            int(row["renewed_with_new_contract"])
            for row in training
            if row["average_delay_days"] is not None
            and float(row["average_delay_days"]) >= 5
        ]

        self.assertGreater(len(low_delay_labels), 50)
        self.assertGreater(len(high_delay_labels), 50)
        self.assertGreater(
            statistics.fmean(low_delay_labels) - statistics.fmean(high_delay_labels),
            0.20,
        )
        self.assertIn(0, low_delay_labels)
        self.assertIn(1, high_delay_labels)

    def test_written_training_columns_follow_feature_contract(self) -> None:
        generator = SyntheticRenewalGenerator(self.make_config())
        generator.generate()
        with tempfile.TemporaryDirectory() as output_dir:
            generator.write_outputs(Path(output_dir))
            with (Path(output_dir) / "feature_columns.json").open(encoding="utf-8") as handle:
                specification = json.load(handle)
            with (Path(output_dir) / "training_dataset.csv").open(
                encoding="utf-8-sig", newline=""
            ) as handle:
                columns = next(csv.reader(handle))

            forbidden = set(specification["explicitly_excluded_as_leakage"])
            self.assertFalse(forbidden.intersection(columns))
            for feature in specification["categorical_features"] + specification["numeric_features"]:
                self.assertIn(feature, columns)


if __name__ == "__main__":
    unittest.main()
