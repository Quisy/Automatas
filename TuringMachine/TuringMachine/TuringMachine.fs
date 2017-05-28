module Classes

open Enums

    type TuringMachine() =
        let mutable _headPosition : int = 0
        let mutable _state : State = State.Q0
        let mutable _tape : string[] = [||]
        let mutable _resultTape : string[] = [||]
        let _stateTransitionArray = [|   
            [| [||]; [|int Sign.Zero|]; [|int Sign.One|]; [|int Sign.Empty|] |]
            [| [|int State.Q0|]; [|int Sign.One; int State.Q2; int Direction.L|];  [|int Sign.Zero; int State.Q1; int Direction.L|]; [|int Sign.None; int State.None; int Direction.None|] |]
            [| [|int State.Q1|]; [|int Sign.Zero; int State.Q2; int Direction.L|]; [|int Sign.One; int State.Q2; int Direction.L|];  [|int Sign.None; int State.None; int Direction.None|] |]
            [| [|int State.Q2|]; [|int Sign.One; int State.Q3; int Direction.L|];  [|int Sign.Zero; int State.Q2; int Direction.L|]; [|int Sign.One; int State.None; int Direction.None|] |]
            [| [|int State.Q3|]; [|int Sign.None; int State.Q3; int Direction.L|]; [|int Sign.None; int State.Q3; int Direction.L|]; [|int Sign.None; int State.None; int Direction.None|] |]
        |]

        let charToInt c = int c - int '0'

        let stateToString state =
            match state with
            |State.Q0 -> "Q0"
            |State.Q1 -> "Q1"
            |State.Q2 -> "Q2"
            |State.Q3 -> "Q3"
            |State.None -> "-"

        let execute(machineValue : int) =
            let transitionValues = _stateTransitionArray.[int _state + 1].[machineValue + 1]
            _state <- enum transitionValues.[1]
            stateToString _state |> printfn "%s" 
            let valueToAppend = if transitionValues.[0] = int Sign.None && machineValue = int Sign.Empty then " " else if transitionValues.[0] = int Sign.None then machineValue.ToString() else transitionValues.[0].ToString()
            let _resultArrayAfter = if _headPosition < _tape.Length then _resultTape.[_headPosition+1..]  else [||]
            let _resultArrayBefore = if _headPosition > 0 then _resultTape.[.._headPosition-1]  else [||]
            _resultTape <- Array.append _resultArrayBefore  _resultArrayAfter
            _resultTape <- [|valueToAppend|]  |> fun x -> Array.append _resultArrayBefore x |> fun x -> Array.append x _resultArrayAfter
            _headPosition <- _headPosition + if transitionValues.[2] = int Direction.L then -1 else if transitionValues.[2] = int Direction.P then 1 else 0
            _tape |> String.concat "" |> printfn "%s"
            _resultTape |> Array.map string |> String.concat "" |> printfn "%s\n"

        member this.runAutomata (signs : char[]) =
            let tempTape = Array.append [|'E'|] signs
            _tape <- tempTape |> Array.map string
            _resultTape <- Array.init _tape.Length (fun x -> "|")
            _state <- State.Q0 
            _headPosition <- _tape.Length - 1
            for sign in Array.rev tempTape do
                let machineValue = if sign.Equals('E') then int Sign.Empty else sign |> charToInt
                execute(machineValue)
                

       

