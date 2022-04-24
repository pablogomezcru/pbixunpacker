# pbixunpacker

PowerBI external tool to unpack pbix files so they can be tracked by version control tools

## Ideas

Inspiration came by need. Having PowerBI to live inside binary files means no source control at all, but also no unit testing, no quality checks, no BPA,... Nothing can be build around a pbix file.

Then this developement started.

Then I found another projects trying to do similar or related things. 
- https://github.com/kodonnell/powerbi-vcs is a very advanced example
- https://github.com/Togusa09/powerbi-vcs-dotnet is the port to dotnet of the previous repo
- https://github.com/pbi-tools/pbix-samples has some examples of unpacked pbix

As per other projects documentation, we found pbix/pbit are in the [Open Packaging Format](https://en.wikipedia.org/wiki/Open_Packaging_Conventions). 

We also discovered PBIT format is simpler than PBIX, so we should start there.

About the binaries inside files, they are in this specifications:
- DataMashup: [MS-QDEF](https://interoperability.blob.core.windows.net/files/MS-QDEFF/%5bMS-QDEFF%5d.pdf)
- DataModel: [MS-XLDM](https://interoperability.blob.core.windows.net/files/MS-XLDM/%5bMS-XLDM%5d.pdf)

### LFS

As pbix files are big, and get bigger by importing data, we should explore LFS + unzipping as the main strategy to store pbix changes.

[LFS](https://git-lfs.github.com/) allows a repo to store big files (mainly binary files) and replace then in the commits by an identifier whereas the real file is stored in another filesystem, limiting the space used in the repo and the real size of the commits. When combined with unzipping we can store the real changes of the pbix file in the unzipped folder, where we'll be able to check who did what and when; and the binary file in the LFS.

### Rezipping



## Commit style

feat -> feature
fix -> bug fix
docs -> documentation
style -> formatting
refactor -> code restructure without changing exterrnal behavior
test -> adding missing tests
chore -> maintenance
init -> initial commit
rearrange -> files moved, added, deleted etc
update -> update code (versions, library compatibility)