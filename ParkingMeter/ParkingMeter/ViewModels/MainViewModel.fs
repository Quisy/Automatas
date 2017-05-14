module ViewModels.MainViewModel

open Models
open ViewModels.BaseViewModel
open Enums

type MainViewModel() =
    inherit BaseViewModel()

    let mutable _value : int = 0
    let mutable _state : ParkingMeterState = ParkingMeterState.Rest

    member this.Value
        with get() = _value
        and set(v : int) = 
            _value <- v
            base.OnPropertyChanged(<@ this.Value @>)

    member this.State
        with get() = _state
        and set(v : ParkingMeterState) = 
            _state <- v
            base.OnPropertyChanged(<@ this.State @>)



