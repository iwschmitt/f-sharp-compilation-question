In the process of learning F#, I have been going through the 2021 Advent of Code problem set. The following three `Invoke` methods are the decompiled assemblies for their respective functions in `Program.fs`. I did not expect any difference between `getFasterAnswer` and `getFastAnswer` but Roslyn compiled these differently in a way that I wouldn't have expected - and also did a pretty staggering number of array allocations.

1) Where should I go, outside of just reading the Roslyn source, to better understand these counterintuitive decisions made by the compiler?
2) If I were to guess, some of the stranger things we are seeing here would get optimised away by RyuJIT after enough iterations, what is the best way to determine this if I created thousands of other test cases to try to force JIT compilation?



```csharp
internal sealed class getFasterAnswer_004020 : OptimizedClosures.FSharpFunc<bool[][], int, bool[][]>
{
	//...
	public override bool[][] Invoke (bool[][] input, int idx)
	{
		while (idx != columnCount && input.Length != 1) {
			bool[][] array = input;
			bool[][] array2 = ArrayModule.Filter (new oneBits_004023 (idx), array);
			if (2 * array2.Length < input.Length) {
				bool[][] array3 = ArrayModule.Filter (new getFasterAnswer_004027_002D1 (isO2, idx), input);
				idx++;
				input = array3;
			} else {
				bool[][] array4 = ArrayModule.Filter (new getFasterAnswer_004029_002D2 (isO2, idx), input);
				idx++;
				input = array4;
			}
		}
		return input;
	}
}
```

```csharp
internal sealed class getFastAnswer_004038 : OptimizedClosures.FSharpFunc<bool[][], int, bool[][]>
{
	//...
	public override bool[][] Invoke (bool[][] input, int idx)
	{
		while (idx != columnCount && input.Length != 1) {
			bool[][] array = input;
			bool[][] array2 = ArrayModule.Filter (new Pipe_0020_00231_0020stage_0020_00231_0020at_0020line_002041_004041 (idx), array);
			bool[][] array3 = array2; // Why has this been allocated? Wasn't in `getFasterAnswer`
			int num = array3.Length;
			if (2 * num < input.Length) {
				bool[][] array4 = ArrayModule.Filter (new getFastAnswer_004045_002D1 (isO2, idx), input);
				idx++;
				input = array4;
			} else {
				bool[][] array5 = ArrayModule.Filter (new getFastAnswer_004047_002D2 (isO2, idx), input);
				idx++;
				input = array5;
			}
		}
		return input;
	}
}
```

```csharp
internal sealed class getSlowAnswer_004056 : OptimizedClosures.FSharpFunc<bool[][], int, bool[][]>
{
	//...
	public override bool[][] Invoke (bool[][] input, int idx)
	{
		while (true) {
			if (idx == columnCount || input.Length == 1) {
				return input;
			}
			bool[][] array = input;
			bool[][] array2 = array; // Why has this been allocated?
			if (array2 == null) {
				break; 
			}
			int[] array3 = new int[array2.Length];
			for (int i = 0; i < array3.Length; i++) {
				int num = i;
				bool[] array4 = array2 [i];
				array3 [num] = (array4 [idx] ? 1 : 0);
			}
			int[] array5 = array3;
			int num2 = ArrayModule.Reduce (oneBitCount_004062.@_instance, array5);
			if (2 * num2 < input.Length) {
				bool[][] array6 = ArrayModule.Filter (new getSlowAnswer_004065_002D1 (isO2, idx), input);
				idx++;
				input = array6;
			} else {
				bool[][] array7 = ArrayModule.Filter (new getSlowAnswer_004067_002D2 (isO2, idx), input);
				idx++;
				input = array7;
			}
		}
		throw new ArgumentNullException ("array"); //Why do flow control this way?
	}
}
```