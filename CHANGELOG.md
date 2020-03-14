## Version 0.9.5 (not released yet)
- Add Deep copy for LoginInformation (**FEATURE**)
- Add Deep copy for LoginInformationSecret (**FEATURE**)
- Add Deep copy for FileEntry (**FEATURE**)
- Add Deep copy for FileEntrySecret (**FEATURE**)
- Add Deep copy for Note (**FEATURE**)
- Add Deep copy for NoteSecret (**FEATURE**)
- Add Deep copy for SymmetricKeyAlgorithm (**FEATURE**)
- Add GetFileContentLengthInBytes for FileEntry and FileEntrySecret (**FEATURE**)

## Version 0.9.4 (released 2020-01-18)
- Add ReplaceLoginInformationSecret to CommonSecretsContainer (**FEATURE**)
- Add ReplaceNoteSecret to CommonSecretsContainer (**FEATURE**)
- Add ReplaceFileEntrySecret to CommonSecretsContainer (**FEATURE**)

## Version 0.9.3 (released 2020-01-04)
- Add GetKeyIdentifier to FileEntrySecret (**FEATURE**)
- Add CanBeDecryptedWithDerivedPassword to LoginInformationSecret (**FEATURE**)
- Add CanBeDecryptedWithDerivedPassword to FileEntrySecret (**FEATURE**)
- Add CanBeDecryptedWithDerivedPassword to NoteSecret (**FEATURE**)
- Rename UpdateUrl -> UpdateURL in LoginInformation (**BREAKING**)
- Fix UpdateEmail in LoginInformation, now it stores data to right place (**FIX**)

## Version 0.9.2 (released 2019-12-26)
- Use correct version number with CommonSecretsContainer constructors (**FIX**)
- Add all parameters constructor for LoginInformation (**FEATURE**)
- Do not accept SHA1 as KeyDerivationPrf for security reason (**BREAKING**)

## Version 0.9.1 (released 2019-11-28)
- Update Microsoft.AspNetCore.Cryptography.KeyDerivation 2.2.0 -> 3.0.0
- Rename GetUrl -> GetURL in LoginInformationSecret (**BREAKING**)
- Add AddLoginInformationSecret with derived password (**FEATURE**)
- Add AddNoteSecret with derived password (**FEATURE**)
- Add AddFileEntrySecret with derived password (**FEATURE**)
- Add invidual setters to LoginInformationSecret, NoteSecret and FileEntrySecret (**FEATURE**)

## Version 0.9.0 (released 2019-11-04)
- First nuget release