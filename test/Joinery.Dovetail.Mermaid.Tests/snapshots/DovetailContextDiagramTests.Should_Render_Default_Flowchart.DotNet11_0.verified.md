```mermaid
---
title: Dovetail Joint Flow
---
flowchart TB
    subgraph sub1 [Setup]
    direction RL
    subgraph sub2 [Joinery.Dovetail.Mermaid.Tests]
    id3["`**Joinery.Dovetail.Mermaid.Tests.MySetupJoint**<br/>HostType: **UnitTest**<br/>Category: **Application**`"]
    end
    end
    subgraph sub6 [Configuration]
    direction RL
    subgraph sub7 [Joinery.Dovetail.Mermaid.Tests]
    id8["`**Joinery.Dovetail.Mermaid.Tests.ConfigurationJoint1**<br/>HostType: **UnitTest**<br/>Category: **Application**`"]
    id9["`**Joinery.Dovetail.Mermaid.Tests.LiveConfigurationJoint**<br/>HostType: **UnitTest**<br/>Category: **Application**`"]
    end
    end
    subgraph sub12 [Service]
    direction RL
    subgraph sub13 [Joinery.Dovetail]
    id14["`**Joinery.Dovetail.Infrastructure.LoggingConnector**<br/>Category: **Application**`"]
    end
    subgraph sub16 [Joinery.Dovetail.Mermaid.Tests]
    id17["`**Joinery.Dovetail.Mermaid.Tests.CoreCategoryJoint**<br/>HostType: **UnitTest**<br/>Category: **Application**`"]
    id18["`**Joinery.Dovetail.Mermaid.Tests.FirstLoggingBridgeJoint**<br/>HostType: **UnitTest**<br/>Category: **Application**`"]
    id19["`**Joinery.Dovetail.Mermaid.Tests.SecondLoggingBridgeJoint**<br/>HostType: **UnitTest**<br/>Category: **Application**`"]
    id20["`**Joinery.Dovetail.Mermaid.Tests.SpecificStepBridgeJoint**<br/>HostType: **UnitTest**<br/>Category: **Application**`"]
    id21["`**Joinery.Dovetail.Mermaid.Tests.TaggedJoint**<br/>HostType: **UnitTest**<br/>Category: **Application**`"]
    id22["`**Joinery.Dovetail.Mermaid.Tests.UnitTestOnlyJoint**<br/>HostType: **UnitTest**<br/>Category: **Application**`"]
    end
    subgraph sub24 [Joinery.Dovetail.Serilog]
    id25["`**Joinery.Dovetail.Serilog.SerilogConfigurationJoint**<br/>Category: **Application**`"]
    id26["`**Joinery.Dovetail.Serilog.SerilogJoint**<br/>Category: **Application**`"]
    end
    end
    subgraph sub29 [Host]
    direction RL
    end
    subgraph sub31 [HostCreated]
    direction RL
    end
    subgraph sub33 [Logging via Joinery.Dovetail.Infrastructure.LoggingConnector, Joinery.Dovetail.Serilog.SerilogJoint]
    direction RL
    subgraph sub34 [Joinery.Dovetail.Mermaid.Tests]
    id35["`**Joinery.Dovetail.Mermaid.Tests.MyLoggingJoint**<br/>HostType: **UnitTest**<br/>Category: **Application**`"]
    id36["`**Joinery.Dovetail.Mermaid.Tests.DependsOnDependsOnPlainJoint**<br/>HostType: **UnitTest**<br/>Category: **Application**`"]
    id37["`**Joinery.Dovetail.Mermaid.Tests.SpecificStepBridgeJoint**<br/>HostType: **UnitTest**<br/>Category: **Application**`"]
    end
    end
    sub6 -.-> sub12
    sub29 -.-> sub31
    sub12 -.-> sub29
    sub12 -.-> sub33
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
    class id26 hostUndefined
    class id35 hostUnitTest
    class id36 hostUnitTest
    class id37 hostUnitTest
```
