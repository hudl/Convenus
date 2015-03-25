
$url = "https://api.spark.io/v1/devices/$env:SparkCoreDeviceId/led"
$postParams = @{access_token=$env:SparkCoreAccessToken;params='LOW'}
Invoke-WebRequest -Uri $url -Method Post -Body $postParams