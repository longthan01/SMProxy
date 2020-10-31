cd ../SMProxy
dotnet publish --output ../Release/ --runtime osx.10.14-x64 --self-contained false
cd ..
cp WindowsProxySetter/bin/Debug/WindowsProxySetter.exe ./Release 