using System;
using CSharp_AUDALF;

namespace CSCommonSecrets
{
	/// <summary>
	/// Common helper class, e.g. to check that derived password contains something
	/// </summary>
	public static class Helpers
	{
		/// <summary>
		/// Generic null and empty check for derived password
		/// </summary>
		public static (bool valid, Exception exception) CheckDerivedPassword(byte[] derivedPassword)
		{
			if (derivedPassword == null)
			{
				return (valid: false, exception: new ArgumentNullException(nameof(derivedPassword)));
			}
			else if (derivedPassword.Length < 1)
			{
				return (valid: false, exception: new ArgumentException($"{nameof(derivedPassword)} does not contain data!"));
			}

			return (valid: true, exception: null);
		}
		
		/// <summary>
		/// Check validity of audalf data
		/// </summary>
		public static (bool valid, Exception exception) CheckAUDALFbytes(byte[] audalfBytes)
		{
			if (audalfBytes == null)
			{
				return (valid: false, exception: new ArgumentNullException(nameof(audalfBytes)));
			}
			else if (audalfBytes.Length < 1)
			{
				return (valid: false, exception: new ArgumentException($"{nameof(audalfBytes)} does not contain data!"));
			}
			else if (!AUDALF_Deserialize.IsAUDALF(audalfBytes))
			{
				return (valid: false, exception: new ArgumentException($"Not valid AUDALF content!"));
			}

			return (valid: true, exception: null);
		} 
	}
}