param($WebhookUrl)

echo "run: Publishing app binaries"

& dotnet publish "$PSScriptRoot/src/Seq.App.HttpRequest" -c Release -o "$PSScriptRoot/src/Seq.App.HttpRequest/obj/publish" --version-suffix=local

if($LASTEXITCODE -ne 0) { exit 1 }    

echo "run: Piping live Seq logs to the app"

& seqcli tail --json | `
    & seqcli app run -d "$PSScriptRoot/src/Seq.App.HttpRequest/obj/publish" `
        -p Url=http://localhost:5050 `
        -p Body="{@m}" `
        -p BodyIsTemplate=True 2>&1 | `
    & seqcli print
