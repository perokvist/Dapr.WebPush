# Dapr.WebPush

![Azure Container Instance Deployment](https://github.com/perokvist/Dapr.WebPush/workflows/Linux_Container_Workflow/badge.svg)

This is a demo of a flow pushing a product web page/fragment to a static site, when product information changes, using dapr pub/sub and bindings.

### Flow

![Context to context communication](assets/webpush_flow.png)

- Recieve "product" updates (pub/sub or input binding)
- Execute templating
- Push to static web (output bindign)

#### TODO
- Cache state
- Push all on changed template

### Product Data

```powershell
 dapr publish --pubsub azurepubsub -t in -d '{\"Title\": \"Fancy Table\", \"Price\": 2500, \"Id\": 4}'
```
Using the dapr CLI we'll post this payload. This could be modified and sent via service bus explorer.

```json
 {"id":"69136027-cb55-47cd-9b32-cdf27b3059f8","source":"push","type":"com.dapr.event.sent","specversion":"1.0","datacontenttype":"application/json","data":{"Title":"Fancy Table","Price":2500,"Id":4},"subject":"00-2c3a831ad26182bf444b131b84945393-792c2bb284a9f319-01","topic":"in","pubsubname":"azurepubsub"}
```

### Application insights

```yaml
apiVersion: dapr.io/v1alpha1
kind: Component
metadata:
  name: native
  namespace: default
spec:
  type: exporters.native
  metadata:
  - name: enabled
    value: "true"
  - name: agentEndpoint
    value: "0.0.0.0:55678"
```

```bash
dapr run {.....} --config config/otel.config 
```

```yaml
apiVersion: dapr.io/v1alpha1
kind: Configuration
metadata:
  name: appconfig
  namespace: default
spec:
  tracing:
    samplingRate: "1"
```

#### Open Telemetry


```yaml
receivers:
  opencensus:
      endpoint: 0.0.0.0:55678
exporters:
  azuremonitor:
  azuremonitor/2:
    endpoint: "https://dc.services.visualstudio.com/v2/track"
    instrumentation_key: "<KEY>"
    maxbatchsize: 100
    maxbatchinterval: 10s
service:
  pipelines:
    traces:
      receivers: [opencensus]
      exporters: [azuremonitor/2]
```

### Deployment

- Building an image
- Push it to a Azure Conatiner Registry
- Deploy it with Dapr through docker compose to ACI
- Using a Azure File share for component config

#### TODO
- Key vault secrets

### Resources

- [Configure a GitHub action to create a container instance](https://docs.microsoft.com/en-us/azure/container-instances/container-instances-github-action)
- [Setting Up Cloud Deployments Using Docker, Azure and Github Actions](https://www.docker.com/blog/setting-up-cloud-deployments-using-docker-azure-and-github-actions/)
- [Dapr Sample - Hello docker compose](https://github.com/dapr/samples/tree/master/hello-docker-compose)
- [Dapr demos](https://github.com/mchmarny/dapr-demos)
