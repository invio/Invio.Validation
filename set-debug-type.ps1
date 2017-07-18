copy 'src\Invio.Validation\project.json' 'src\Invio.Validation\project.json.bak'
$project = Get-Content 'src\Invio.Validation\project.json.bak' -raw | ConvertFrom-Json
$project.buildOptions.debugType = "full"
$project | ConvertTo-Json  | set-content 'src\Invio.Validation\project.json'