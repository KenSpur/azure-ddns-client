#!/bin/bash

# exit when any command fails
set -e

# Initialize a variable to keep track of unset variables
unset_vars=""

# Function to check if a variable is set
check_var() {
    if [ -z "${!1}" ]; then
        unset_vars="$unset_vars\n$1"
    fi
}

# Check each variable
check_var "AZURE_OPTIONS__TENANT_ID"
check_var "AZURE_OPTIONS__CLIENT_ID"
check_var "AZURE_OPTIONS__CLIENT_SECRET"
check_var "AZURE_OPTIONS__SUBSCRIPTION_ID"
check_var "AZURE_OPTIONS__RESOURCE_GROUP_NAME"
check_var "AZURE_OPTIONS__DNS_ZONE_NAME"
check_var "WHAT_IS_MY_IP_SERVER_OPTIONS__BASE_ADDRESS"

# If any variables are unset, print them and exit
if [ -n "$unset_vars" ]; then
    echo -e "The following environment variables are not set:$unset_vars"
    exit 1
fi

# Run your command if all variables are set
dotnet AzureDDNSClient.dll