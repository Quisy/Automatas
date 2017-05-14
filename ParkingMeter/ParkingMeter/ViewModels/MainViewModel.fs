module ViewModels.MainViewModel

open Enums
open Models
open System.Windows.Input
open ViewModels.BaseViewModel

type MainViewModel() as mainVM = 
    inherit BaseViewModel()
    let mutable _value : int = 0
    let mutable _state : ParkingMeterState = ParkingMeterState.Q0
    let mutable _visitedSates : List<ParkingMeterState> = [ ParkingMeterState.Q0 ]
    
    let createCommand action canExecute = 
        let event1 = Event<_, _>()
        { new ICommand with
              member this.CanExecute(obj) = canExecute (obj)
              member this.Execute(obj) = action (obj)
              member this.add_CanExecuteChanged (handler) = event1.Publish.AddHandler(handler)
              member this.remove_CanExecuteChanged (handler) = event1.Publish.AddHandler(handler) }
    
    let addToList (list : List<ParkingMeterState>) (element : ParkingMeterState) = element :: list
    
    member this.Value 
        with get () = _value
        and set (v : int) = 
            _value <- v
            base.OnPropertyChanged(<@ this.Value @>)
    
    member this.State 
        with get () = _state
        and set (v : ParkingMeterState) = 
            _state <- v
            base.OnPropertyChanged(<@ this.State @>)
    
    member this.StateRoad = _visitedSates |> List.fold (fun string s -> " > " + s.ToString() + string) ""
    member this.AddToValue(value : int) = this.Value <- this.Value + value
    
    member this.AddOne = 
        createCommand (fun _ -> 
            this.AddToValue(1)
            this.manageState()) (fun _ -> true)
    
    member this.AddTwo = 
        createCommand (fun _ -> 
            this.AddToValue(2)
            this.manageState()) (fun _ -> true)
    
    member this.AddFive = 
        createCommand (fun _ -> 
            this.AddToValue(5)
            this.manageState()) (fun _ -> true)
    
    member private this.manageState = 
        let currentValue = this.Value
        match currentValue with
        | 1 -> fun () -> this.changeState (ParkingMeterState.Q1)
        | 2 -> fun () -> this.changeState (ParkingMeterState.Q2)
        | 3 -> fun () -> this.changeState (ParkingMeterState.Q3)
        | 4 -> fun () -> this.changeState (ParkingMeterState.Q4)
        | 5 -> fun () -> this.changeState (ParkingMeterState.Q5)
        | 6 -> fun () -> this.changeState (ParkingMeterState.Q6)
        | currentValue when this.Value = 7 -> 
            fun _ -> 
                this.changeState (ParkingMeterState.Q7)
                this.Value <- 0
        | currentValue when this.Value > 7 -> 
            fun () -> 
                this.changeState (ParkingMeterState.Q8)
                this.Value <- 0
        | _ -> fun () -> this.State <- ParkingMeterState.Q0
    
    member private this.changeState = 
        fun (state : ParkingMeterState) -> 
            this.State <- state
            _visitedSates <- addToList _visitedSates state
            mainVM.OnPropertyChanged(<@ this.StateRoad @>) |> ignore
