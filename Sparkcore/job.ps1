<#
.Synopsis
   Call the Convenus API to get the room status.
.DESCRIPTION
   Given a list of room, call the Convenus API to get the room status.
.EXAMPLE
   "bojackson@hudl.com", "gordie@hudl.com" | Get-RoomStatus
#>
function Get-RoomStatus
{
    [CmdletBinding()]
    Param
    (
        # List of room
        [Parameter(Mandatory=$true,
                   ValueFromPipeline=$true,
                   ValueFromPipelineByPropertyName=$true,
                   Position=0)]
        [string[]]
        $Room
    )

    Begin
    {
        $result = @{}
    }
    Process
    {
        $url = "http://calendars/api/rooms/$Room/5"
        $status = Invoke-RestMethod $url
        $result += @{"$Room" = $status}
    }
    End
    {
        return $result
    }
}

$jsonFile = "C:\Temp\devices.json"
$jsonObj = Get-Content $jsonFile -Raw | ConvertFrom-Json
$cores=@{}
foreach ($p in $jsonObj.cores.psobject.properties.name){
    $cores[$p]=$jsonObj.cores.$p
}

$result = "bojackson@hudl.com" | Get-RoomStatus
foreach ($r in $result.GetEnumerator()){
    $deviceId = $cores[$($r.Name)]
    Write-Output "Device Id: $deviceId"
    $url = "https://api.spark.io/v1/devices/$deviceId/led"
    $postParams = @{access_token=$env:SparkCoreAccessToken;params='LOW'}
    Invoke-RestMethod $url -Method Post -Body $postParams
}