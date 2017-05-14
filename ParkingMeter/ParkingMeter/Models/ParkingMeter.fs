module Models

open Enums

type ParkingMeter = {
    State: ParkingMeterState
    Value: int
}

