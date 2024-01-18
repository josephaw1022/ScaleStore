# Obtain access token
$tokenResponse = Invoke-RestMethod -Uri "http://localhost:8080/auth/realms/master/protocol/openid-connect/token" -Method Post -Body @{
    client_id  = "admin-cli"
    username   = "admin"
    password   = "admin"
    grant_type = "password"
}

$accessToken = $tokenResponse.access_token

# Desired realm name
$desiredRealm = "mynewrealm"

# Define the user details
$newUserDetails = @{
    username    = "newusername"
    email       = "user@example.com"
    enabled     = $true
    firstName   = "New"
    lastName    = "User"
    credentials = @(
        @{
            type      = "password"
            value     = "password"
            temporary = $false
        }
    )
    # Add other user configurations as needed
}

$jsonNewUserDetails = $newUserDetails | ConvertTo-Json

# Send the request to create a new user in the realm
$createUserResponse = Invoke-RestMethod -Uri "http://localhost:8080/auth/admin/realms/$desiredRealm/users" -Method Post -Headers @{
    Authorization  = "Bearer $accessToken"
    "Content-Type" = "application/json"
} -Body $jsonNewUserDetails

# Check response status
if ($createUserResponse.StatusCode -eq 201) {
    Write-Output "User '$($newUserDetails.username)' created successfully in realm '$desiredRealm'."
} else {
    Write-Output "Failed to create user. Status code: $($createUserResponse.StatusCode)"
}

# Get existing realms
$existingRealms = Invoke-RestMethod -Uri "http://localhost:8080/auth/admin/realms" -Method Get -Headers @{
    Authorization = "Bearer $accessToken"
}

# Check if the desired realm already exists
$realmExists = $existingRealms | Where-Object { $_.realm -eq $desiredRealm }

if ($realmExists) {
    Write-Output "Realm '$desiredRealm' already exists. Skipping creation."
} else {
    # Create a new realm
    $newRealmDetails = @{
        realm   = $desiredRealm
        enabled = $true
        # Add other realm configurations as needed
    } | ConvertTo-Json

    # Send the request to create a new realm
    $createRealmResponse = Invoke-RestMethod -Uri "http://localhost:8080/auth/admin/realms" -Method Post -Headers @{
        Authorization  = "Bearer $accessToken"
        "Content-Type" = "application/json"
    } -Body $newRealmDetails

    # Print the create realm response
    Write-Output "Create Realm Response:"
    Write-Output $createRealmResponse
}

# Define the client details
$newClientDetails = @{
    clientId = "mynewclient"
    name     = "My New Client"
    enabled  = $true
    # Add other client configurations as needed
}

$jsonNewClientDetails = $newClientDetails | ConvertTo-Json

# Check if the client already exists in the realm
$existingClients = Invoke-RestMethod -Uri "http://localhost:8080/auth/admin/realms/$desiredRealm/clients" -Method Get -Headers @{
    Authorization = "Bearer $accessToken"
}

$desiredClient = $newClientDetails
$desiredClientId = $newClientDetails["clientId"]

# Initialize clientId variable
$clientId = $null

# Iterate through existing clients to find the matching clientId
foreach ($client in $existingClients) {
    if ($client.clientId -eq $newClientDetails.clientId) {
        $clientId = $client.id
        break
    }
}

if (![string]::IsNullOrEmpty($clientId)) {
    Write-Output "Client '$($newClientDetails.clientId)' already exists in realm '$desiredRealm'."
} else {
    # Send the request to create a new client
    $createClientResponse = Invoke-RestMethod -Uri "http://localhost:8080/auth/admin/realms/$desiredRealm/clients" -Method Post -Headers @{
        Authorization  = "Bearer $accessToken"
        "Content-Type" = "application/json"
    } -Body $jsonNewClientDetails

    # Get the ID of the newly created client
    $clientId = $createClientResponse.id

    Write-Output "Client '$($newClientDetails.clientId)' created in realm '$desiredRealm'."
}

# Generate a new client secret, regardless of whether the client was just created or already existed
$clientSecretResponse = Invoke-RestMethod -Uri "http://localhost:8080/auth/admin/realms/$desiredRealm/clients/$clientId/client-secret" -Method Post -Headers @{
    Authorization  = "Bearer $accessToken"
    "Content-Type" = "application/json"
} -Body "{}"

# Print the new client secret
Write-Output "New Client Secret for '$($newClientDetails.clientId)':"
Write-Output $clientSecretResponse.value