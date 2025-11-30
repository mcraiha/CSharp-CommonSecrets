using System;
using System.Collections.Generic;

namespace CSCommonSecrets;

/// <summary>
/// CommonSecretsContainer stores all other data. e.g. KeyDerivationFunctionEntries, LoginInformations etc.
/// </summary>
public sealed partial class CommonSecretsContainer
{
	/// <summary>
	/// Current common secrets container specification version
	/// </summary>
	public static readonly int currentVersionNumber = 1;

	/// <summary>
	/// Specification version of this Common Secrets Container instance
	/// </summary>
	public int version { get; set; }

	/// <summary>
	/// All key derivation function entries in list
	/// </summary>
	public List<KeyDerivationFunctionEntry> keyDerivationFunctionEntries { get; set; } = new List<KeyDerivationFunctionEntry>();

	/// <summary>
	/// List of login informations (plain text ones)
	/// </summary>
	public List<LoginInformation> loginInformations { get; set; } = new List<LoginInformation>();

	/// <summary>
	/// List of login information secrets
	/// </summary>
	public List<LoginInformationSecret> loginInformationSecrets { get; set; } = new List<LoginInformationSecret>();

	/// <summary>
	/// List of notes (plain text ones)
	/// </summary>
	public List<Note> notes { get; set; } = new List<Note>();

	/// <summary>
	/// List of note secrets
	/// </summary>
	public List<NoteSecret> noteSecrets { get; set; } = new List<NoteSecret>();

	/// <summary>
	/// List of files (plain text ones)
	/// </summary>
	public List<FileEntry> files { get; set; } = new List<FileEntry>();

	/// <summary>
	/// List of file secrets
	/// </summary>
	public List<FileEntrySecret> fileSecrets { get; set; } = new List<FileEntrySecret>();

	/// <summary>
	/// List of contacts (plain text ones)
	/// </summary>
	public List<Contact> contacts { get; set; } = new List<Contact>();

	/// <summary>
	/// List of contact secrets
	/// </summary>
	public List<ContactSecret> contactSecrets { get; set; } = new List<ContactSecret>();

	/// <summary>
	/// List of payment cards (plain text ones)
	/// </summary>
	public List<PaymentCard> paymentCards { get; set; } = new List<PaymentCard>();

	/// <summary>
	/// List of payment card secrets
	/// </summary>
	public List<PaymentCardSecret> paymentCardSecrets { get; set; } = new List<PaymentCardSecret>();

	/// <summary>
	/// List of history entries
	/// </summary>
	/// <remarks>These are optional</remarks>
	public List<History> history { get; set; } = new List<History>();

	/// <summary>
	/// List of history secret entries
	/// </summary>
	/// <remarks>These are optional</remarks>
	public List<HistorySecret> historySecrets { get; set; } = new List<HistorySecret>();

	/// <summary>
	/// Constructor without parameters for creating empty CommonSecretsContainer
	/// </summary>
	public CommonSecretsContainer()
	{
		this.version = currentVersionNumber;
	}

	/// <summary>
	/// Constructor for creating CommonSecretsContainer with one Key Derivation Function
	/// </summary>
	/// <param name="primaryKDF">"Primary" key derivation function</param>
	public CommonSecretsContainer(KeyDerivationFunctionEntry primaryKDF)
	{
		this.version = currentVersionNumber;
		this.keyDerivationFunctionEntries.Add(primaryKDF);
	}

	#region Helpers

	/// <summary>
	/// Get all identifiers of KeyDerivationFunctionEntries
	/// </summary>
	/// <returns>IEnumerable collection of identifiers</returns>
	public IEnumerable<string> GetKeyDerivationFunctionEntryIdentifiers()
	{
		List<string> returnList = new List<string>();

		foreach (KeyDerivationFunctionEntry kdfe in keyDerivationFunctionEntries)
		{
			returnList.Add(kdfe.GetKeyIdentifier());
		}

		return returnList;
	}

	/// <summary>
	/// Find KeyDerivationFunctionEntry with key identifier
	/// </summary>
	/// <param name="keyIdentifier">Key identifier to seek</param>
	/// <returns>KeyDerivationFunctionEntry if match is found; null otherwise</returns>
	public KeyDerivationFunctionEntry FindKeyDerivationFunctionEntryWithKeyIdentifier(string keyIdentifier)
	{
		KeyDerivationFunctionEntry returnValue = null;

		foreach (KeyDerivationFunctionEntry kdfe in keyDerivationFunctionEntries)
		{
			if (keyIdentifier == kdfe.GetKeyIdentifier())
			{
				returnValue = kdfe;
				break;
			}
		}

		return returnValue;
	}


	private (bool success, string possibleError) MandatoryChecks(object checkForNull, string objectToCheckForError, string keyIdentifier, string password)
	{
		(bool commonCheckResult, string possibleCommonCheckError) = this.MandatoryCommonChecks(checkForNull, objectToCheckForError, keyIdentifier);

		if (!commonCheckResult)
		{
			return (commonCheckResult, possibleCommonCheckError);
		}

		if (string.IsNullOrEmpty(password))
		{
			return (false, "Password must contain something!");
		}

		return (success: true, possibleError: "");
	}

	private (bool success, string possibleError) MandatoryChecks(object checkForNull, string objectToCheckForError, string keyIdentifier, byte[] derivedPassword)
	{
		(bool commonCheckResult, string possibleCommonCheckError) = this.MandatoryCommonChecks(checkForNull, objectToCheckForError, keyIdentifier);

		if (!commonCheckResult)
		{
			return (commonCheckResult, possibleCommonCheckError);
		}

		(bool derivedPasswordValid, Exception exception) = Helpers.CheckDerivedPassword(derivedPassword);

		if (!derivedPasswordValid)
		{
			return (derivedPasswordValid, exception.ToString());
		}

		return (success: true, possibleError: "");
	}

	private (bool success, string possibleError) MandatoryCommonChecks(object checkForNull, string objectToCheckForError, string keyIdentifier)
	{
		if (checkForNull == null)
		{
			return (success: false, possibleError: $"{objectToCheckForError} cannot be null");
		}

		KeyDerivationFunctionEntry kdfe = this.FindKeyDerivationFunctionEntryWithKeyIdentifier(keyIdentifier);

		if (kdfe == null)
		{
			return (success: false, possibleError: $"Cannot find key identifier matching to: {keyIdentifier}");
		}

		return (success: true, possibleError: "");
	}

	#endregion // Helpers
}
