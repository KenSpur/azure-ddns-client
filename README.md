# 🖥️ Azure DDNS Client

Easily synchronize the `@` A record in your Azure DNS Zone with the dynamic public IP of your host using the **azure-ddns-client**.

> 🌐 Utilizes [What Is My IP Server](https://github.com/KenSpur/what-is-my-ip-server) to retrieve the host's current public IP address.

## 🌉 Use-Case: VPN Connection Without a Static IP

The **azure-ddns-client** proves its utility in my setup where I establish a site-to-site VPN connection between my local network and Azure. This circumvents the necessity for a static IP address, providing a cost-efficient solution by dynamically updating the DNS record upon changes in my local network's public IP.

🥧 **Runs on Raspberry Pi!** The CronJob is successfully executed in a lightweight k3s cluster on a Raspberry Pi.

## 🦑 Deploy as a K8s/K3s CronJob

### 1️⃣ Configure the CronJob

Utilize the following YAML configuration to set up the CronJob in your Kubernetes cluster.

```yaml
apiVersion: batch/v1
kind: CronJob
metadata:
  name: azure-ddns-client
  namespace: [Your-Namespace]
spec:
  schedule: "*/15 * * * *" # Modify as needed
  jobTemplate:
    spec:
      template:
        spec:
          containers:
          - name: azure-ddns-client
            image: ghcr.io/kenspur/azure-ddns-client:v0.1.1
            env:
            - name: AZURE_OPTIONS__TENANT_ID
              value: [Your-Tenant-ID]
            - name: AZURE_OPTIONS__CLIENT_ID
              value: [Your-Client-ID]
            - name: AZURE_OPTIONS__CLIENT_SECRET
              valueFrom:
                secretKeyRef:
                  name: azure-ddns-secret
                  key: client-secret
            - name: AZURE_OPTIONS__SUBSCRIPTION_ID
              value: [Your-Subscription-ID]
            - name: AZURE_OPTIONS__RESOURCE_GROUP_NAME
              value: [Your-Resource-Group-Name]
            - name: AZURE_OPTIONS__DNS_ZONE_NAME
              value: [Your-DNS-Zone-Name]
            - name: WHAT_IS_MY_IP_SERVER_OPTIONS__BASE_ADDRESS
              value: [Your-IP-Server-Base-Address]
          restartPolicy: OnFailure
```

### 2️⃣ Manage Secrets Securely

Store sensitive information, such as your client secret, in a Kubernetes secret as demonstrated below.

```yaml
apiVersion: v1
kind: Secret
metadata:
  name: azure-ddns-secret
  namespace: [Your-Namespace]
type: Opaque
data:
  client-secret: [Base64-Encoded-Secret]
``````
⚠️ Ensure that all placeholder values are appropriately replaced before deploying to your cluster.
