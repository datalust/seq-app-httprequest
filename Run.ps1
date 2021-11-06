param($WebhookUrl)

echo "run: Publishing app binaries"

& dotnet publish "$PSScriptRoot/src/Seq.App.Http" -c Release -o "$PSScriptRoot/src/Seq.App.Http/obj/publish" --version-suffix=local

if($LASTEXITCODE -ne 0) { exit 1 }    

echo "run: Piping live Seq logs to the app"

& seqcli tail --json | & seqcli app run -d "$PSScriptRoot/src/Seq.App.Http/obj/publish" -p Url=http://localhost:5050 2>&1 | & seqcli print
