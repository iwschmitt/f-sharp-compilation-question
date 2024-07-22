let testInput =
    [| "00100"
       "11110"
       "10110"
       "10111"
       "10101"
       "01111"
       "00111"
       "11100"
       "10000"
       "11001"
       "00010"
       "01010" |]

[<TailCall>]
let getFasterAnswer (input: bool[][]) isO2 =
    let columnCount = input.[0].Length

    let rec getFasterAnswer (input: bool[][]) idx =
        if idx = columnCount || input.Length = 1 then
            input
        else
            let oneBits = input |> Array.filter (fun row -> row.[idx])

            if (2 * oneBits.Length) < input.Length then // Low bit count dominates
                //if (2 * oneBitCount) < input.Length then // Low bit count dominates
                getFasterAnswer (Array.filter (fun row -> not isO2 = row.[idx]) input) (idx + 1)
            else //High bit count dominates
                getFasterAnswer (Array.filter (fun row -> isO2 = row.[idx]) input) (idx + 1)

    getFasterAnswer input 0

[<TailCall>]
let getFastAnswer (input: bool[][]) isO2 =
    let columnCount = input.[0].Length

    let rec getFastAnswer (input: bool[][]) idx =
        if idx = columnCount || input.Length = 1 then
            input
        else
            let oneBits = input |> Array.filter (fun row -> row.[idx]) |> (fun i -> i.Length)

            if (2 * oneBits) < input.Length then // Low bit count dominates
                //if (2 * oneBitCount) < input.Length then // Low bit count dominates
                getFastAnswer (Array.filter (fun row -> not isO2 = row.[idx]) input) (idx + 1)
            else //High bit count dominates
                getFastAnswer (Array.filter (fun row -> isO2 = row.[idx]) input) (idx + 1)

    getFastAnswer input 0

[<TailCall>]
let getSlowAnswer (input: bool[][]) isO2 =
    let columnCount = input.[0].Length

    let rec getSlowAnswer (input: bool[][]) idx =
        if idx = columnCount || input.Length = 1 then
            input
        else
            let oneBitCount =
                input
                |> Array.map (fun row -> if row.[idx] then 1 else 0)
                |> Array.reduce (fun a b -> a + b)

            if (2 * oneBitCount) < input.Length then // Low bit count dominates
                getSlowAnswer (Array.filter (fun row -> not isO2 = row.[idx]) input) (idx + 1)
            else //High bit count dominates
                getSlowAnswer (Array.filter (fun row -> isO2 = row.[idx]) input) (idx + 1)

    getSlowAnswer input 0

let gasInput (input: string[]) =
    input
    |> Array.map (fun line -> line.ToCharArray() |> Array.map (fun c -> c = '1'))

[<EntryPoint>]
let main argv =
    let o2a = getFasterAnswer (gasInput testInput) true
    let o2b = getFastAnswer (gasInput testInput) true
    let o2c = getSlowAnswer (gasInput testInput) true
    0
