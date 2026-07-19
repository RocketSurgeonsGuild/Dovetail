```mermaid
---
title: Dovetail Joint Flow
---
flowchart TB
    subgraph sub1 [Setup]
    direction RL
    subgraph sub2 [Dovetail.Mermaid.Tests]
    id3["`**Dovetail.Mermaid.Tests.MySetupJoint**<br/>HostType: **UnitTest**<br/>Category: **Application**`"]
    end
    end
    subgraph sub6 [Configuration]
    direction RL
    subgraph sub7 [Dovetail.Mermaid.Tests]
    id8["`**Dovetail.Mermaid.Tests.ConfigurationJoint1**<br/>HostType: **UnitTest**<br/>Category: **Application**`"]
    id9["`**Dovetail.Mermaid.Tests.LiveConfigurationJoint**<br/>HostType: **UnitTest**<br/>Category: **Application**`"]
    end
    end
    subgraph sub12 [Service]
    direction RL
    subgraph sub13 [Dovetail]
    id14["`**Dovetail.Infrastructure.LoggingConnector**<br/>Category: **Application**`"]
    end
    subgraph sub16 [Dovetail.Mermaid.Tests]
    id17["`**Dovetail.Mermaid.Tests.CoreCategoryJoint**<br/>HostType: **UnitTest**<br/>Category: **Application**`"]
    id18["`**Dovetail.Mermaid.Tests.FirstLoggingBridgeJoint**<br/>HostType: **UnitTest**<br/>Category: **Application**`"]
    id19["`**Dovetail.Mermaid.Tests.SecondLoggingBridgeJoint**<br/>HostType: **UnitTest**<br/>Category: **Application**`"]
    id20["`**Dovetail.Mermaid.Tests.SpecificStepBridgeJoint**<br/>HostType: **UnitTest**<br/>Category: **Application**`"]
    id21["`**Dovetail.Mermaid.Tests.TaggedJoint**<br/>HostType: **UnitTest**<br/>Category: **Application**`"]
    id22["`**Dovetail.Mermaid.Tests.UnitTestOnlyJoint**<br/>HostType: **UnitTest**<br/>Category: **Application**`"]
    end
    subgraph sub24 [Dovetail.OpenTelemetry]
    id25["`**Dovetail.OpenTelemetry.OpenTelemetryConnector**<br/>Category: **Application**`"]
    end
    subgraph sub27 [Dovetail.Serilog]
    id28["`**Dovetail.Serilog.SerilogConfigurationJoint**<br/>Category: **Application**`"]
    id29["`**Dovetail.Serilog.SerilogJoint**<br/>Category: **Application**`"]
    end
    end
    subgraph sub32 [Host]
    direction RL
    end
    subgraph sub34 [HostCreated]
    direction RL
    end
    subgraph sub36 [OpenTelemetry via Dovetail.OpenTelemetry.OpenTelemetryConnector]
    direction RL
    end
    subgraph sub38 [Logging via Dovetail.Infrastructure.LoggingConnector, Dovetail.Serilog.SerilogJoint]
    direction RL
    subgraph sub39 [Dovetail.Mermaid.Tests]
    id40["`**Dovetail.Mermaid.Tests.MyLoggingJoint**<br/>HostType: **UnitTest**<br/>Category: **Application**`"]
    id41["`**Dovetail.Mermaid.Tests.DependsOnDependsOnPlainJoint**<br/>HostType: **UnitTest**<br/>Category: **Application**`"]
    id42["`**Dovetail.Mermaid.Tests.SpecificStepBridgeJoint**<br/>HostType: **UnitTest**<br/>Category: **Application**`"]
    end
    end
    sub6 -.-> sub12
    sub32 -.-> sub34
    sub12 -.-> sub32
    sub12 -.-> sub38
    sub12 -.-> sub36
    sub1 -.-> sub6
    classDef hostUndefined text-align:left,fill:#eceff1,stroke:#607d8b,color:#263238
    classDef hostLive text-align:left,fill:#c8e6c9,stroke:#2e7d32,color:#1b5e20
    classDef hostUnitTest text-align:left,fill:#ffe0b2,stroke:#ef6c00,color:#e65100
    class id3 hostUnitTest
    class id8 hostUnitTest
    class id9 hostUnitTest
    class id14 hostUndefined
    class id17 hostUnitTest
    class id18 hostUnitTest
    class id19 hostUnitTest
    class id20 hostUnitTest
    class id21 hostUnitTest
    class id22 hostUnitTest
    class id25 hostUndefined
    class id28 hostUndefined
    class id29 hostUndefined
    class id40 hostUnitTest
    class id41 hostUnitTest
    class id42 hostUnitTest
```
