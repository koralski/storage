This is a scratch project to put some experimental files. Nothing interesting for the public.

Powershell deployment:

1. Login with your azure account:
Login-AzureRmAccount

2. Create a new resource group:
New-AzureRmResourceGroup -Name "containersRG1" -Location "West US"

3. Deploy the templates (from local files):
New-AzureRmResourceGroupDeployment -Name "myDeployment" -TemplateParameterFile .\parameters.json -TemplateFile .\deploy.json -ResourceGroupName "containersRG1"
