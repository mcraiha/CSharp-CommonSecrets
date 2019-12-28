## Version 0.9.3 (not released yet)
- Add GetKeyIdentifier to FileEntrySecret (**FEATURE**)

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