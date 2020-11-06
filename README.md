# Dapr.WebPush

![Azure Container Instance Deployment](https://github.com/perokvist/Dapr.WebPush/workflows/Linux_Container_Workflow/badge.svg)

```powershell
 dapr publish --pubsub azurepubsub -t in -d '{\"Title\": \"Fancy Table\", \"Price\": 2500, \"Id\": 4}'
```

```json
 {"id":"69136027-cb55-47cd-9b32-cdf27b3059f8","source":"push","type":"com.dapr.event.sent","specversion":"1.0","datacontenttype":"application/json","data":{"Title":"Fancy Table","Price":2500,"Id":4},"subject":"00-2c3a831ad26182bf444b131b84945393-792c2bb284a9f319-01","topic":"in","pubsubname":"azurepubsub"}
```

### Flow

- Recieve "product" updates (pub/sub or input binding)
- Execute templating
- Push to static web (output bindign)

#### TODO
- Cache state
- Push all on changed template

### Deployment

- Building an image
- Push it to a Azure Conatiner Registry
- Deploy it with Dapr through docker compose to ACI
- Using a Azure File share for component config

#### TODO
- Application Insights
- Key vault secrets

### Resources

- [Configure a GitHub action to create a container instance](https://docs.microsoft.com/en-us/azure/container-instances/container-instances-github-action)
- [Setting Up Cloud Deployments Using Docker, Azure and Github Actions](https://www.docker.com/blog/setting-up-cloud-deployments-using-docker-azure-and-github-actions/)
- [Dapr Sample - Hello docker compose](https://github.com/dapr/samples/tree/master/hello-docker-compose)
- [Dapr demos](https://github.com/mchmarny/dapr-demos)
