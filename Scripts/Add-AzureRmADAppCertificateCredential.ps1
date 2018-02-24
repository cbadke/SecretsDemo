param(
	[Parameter(Mandatory=$true)]
	[string]$AppId,
	[Parameter(Mandatory=$true)]
	[string]$CerPath
)

$CerPath = (Resolve-Path $CerPath).Path

$x509 = New-Object System.Security.Cryptography.X509Certificates.X509Certificate2
$x509.Import($CerPath)
$credValue = [System.Convert]::ToBase64String($x509.GetRawCertData())

Login-AzureRmAccount

New-AzureRmADAppCredential -ApplicationId $AppId -CertValue $credValue -StartDate $x509.NotBefore -EndDate $x509.NotAfter