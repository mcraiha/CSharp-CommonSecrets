using System.Collections.Generic;

namespace Tests
{
	// Simple class for mutating input
	public static class Mutator
	{
		public static byte[] CreateMutatedByteArray(byte[] inputArray)
		{
			List<byte> returnArray = new List<byte>(inputArray);
			if (returnArray[0] < 255)
			{
				returnArray[0]++;
			}
			else
			{
				returnArray[0] = 0;
			}
			return returnArray.ToArray();
		}
	}
}