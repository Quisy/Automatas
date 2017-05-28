// Learn more about F# at http://fsharp.org
// See the 'F# Tutorial' project for more help.

open Classes

[<EntryPoint>]
let main argv = 
    let turingMachine = new TuringMachine()
    let binaryNumber : string = argv.[0]
    turingMachine.runAutomata(binaryNumber.ToCharArray())
    0 // return an integer exit code


