# Reference

Dovetail is extended through MSBuild items. You declare one in a project file, and the build writes the C# for it into `obj/` before
the compiler runs. MSBuild targets expand the items, not the Roslyn source generator, so there is nothing to configure and nothing
to call at run time. These pages list every metadata value an item reads, with its default, so you do not have to read the
`.targets` files.

[DovetailJoint](./dovetail-joint/)
    [DovetailHost](./dovetail-host/)
    [DovetailTemplate](./dovetail-template/)
For how to write the joints these items produce, and how Dovetail orders and runs them, see [Defining Dovetails](../concepts/defining-joints/).