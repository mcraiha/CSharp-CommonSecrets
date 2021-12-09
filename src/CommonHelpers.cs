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

		/// <summary>
		/// Get single value from encrypted audalf dictionary
		/// </summary>
		/// <param name="audalfData">Encrypted AUDALF data</param>
		/// <param name="algorithm">SymmetricKeyAlgorithm to use</param>
		/// <param name="derivedPassword">Derived password as byte array</param>
		/// <param name="key">Dictionary key</param>
		/// <param name="deserializationSettings">Deserialization settings</param>
		/// <returns>Object containing the data</returns>
		public static object GetSingleValue(byte[] audalfData, SymmetricKeyAlgorithm algorithm, byte[] derivedPassword, string key, DeserializationSettings deserializationSettings)
		{
			var passwordCheck = Helpers.CheckDerivedPassword(derivedPassword);

			if (!passwordCheck.valid)
			{
				throw passwordCheck.exception;
			}

			// Try to decrypt the binary
			byte[] decryptedAUDALF = algorithm.DecryptBytes(audalfData, derivedPassword);

			var audalfCheck = Helpers.CheckAUDALFbytes(decryptedAUDALF);

			if (!audalfCheck.valid)
			{
				throw audalfCheck.exception;
			}

			return AUDALF_Deserialize.DeserializeSingleValue<string, object>(decryptedAUDALF, key, settings: deserializationSettings);
		}
	}
}