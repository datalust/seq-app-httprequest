param($WebhookUrl)

echo "run: Publishing app binaries"

& dotnet publish "$PSScriptRoot/Seq.App.Teams" -c Release -o "$PSScriptRoot/Seq.App.Teams/obj/publish" --version-suffix=local

if($LASTEXITCODE -ne 0) { exit 1 }    

echo "run: Piping live Seq logs to the app"

& seqcli tail --json | & seqcli app run -d "$PSScriptRoot/Seq.App.Teams/obj/publish" -p TraceMessage=True -p TeamsBaseUrl="$WebhookUrl" 2>&1 | & seqcli print
