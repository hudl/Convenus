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

$data = @{}
$data += @{"bojackson@hudl.com" = $env:SparkCoreDeviceId}
$result = "bojackson@hudl.com" | Get-RoomStatus
foreach ($r in $result.GetEnumerator()){
    $deviceId = $data[$($r.Name)]
    #Write-Output "$($r.Name): $($r.Value)"
    #Write-Output "Device Id: $deviceId"
    $url = "https://api.spark.io/v1/devices/$deviceId/led"
    $postParams = @{access_token=$env:SparkCoreAccessToken;params='HIGH'}
    Invoke-WebRequest -Uri $url -Method Post -Body $postParams
}