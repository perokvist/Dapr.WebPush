# Dapr.WebPush

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