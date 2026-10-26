using Dovetail.Joints;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

// Exports is generated: a formatter that runs without the generator can neither resolve it nor suggest simplifying its name, so the
// fully qualified reference would flip between IDE0002 and a removed using on every automatic lint run.
#pragma warning disable IDE0002

namespace Dovetail.Configuration.Parity.Tests;

/// <summary>The three binders compared: Dovetail's generated one, Microsoft's generated one, and the reflection binder.</summary>
internal static class Binders
{
    /// <summary>
    ///     Binds through the joint this assembly exports, as an application does: every <c>[DovetailConfiguration]</c> section is
    ///     registered, and <typeparamref name="T" /> is read from <see cref="IOptions{TOptions}" />.
    /// </summary>
    public static T Dovetail<T>(IConfiguration configuration) where T : class
    {
        using var provider = DovetailServices(configuration).BuildServiceProvider();
        return provider.GetRequiredService<IOptions<T>>().Value;
    }

    public static ServiceCollection DovetailServices(IConfiguration configuration)
    {
        var services = new ServiceCollection();
        services.AddSingleton(configuration);
        foreach (var joint in global::Dovetail.Configuration.Parity.Tests.Rivets.Exports.Joints().Select(z => z.Joint).OfType<IServiceJoint>())
        {
            joint.Register(null!, services);
        }

        return services;
    }

    /// <summary>The reflection binder: a call with an open type argument isn't intercepted by Microsoft's generator (SYSLIB1104).</summary>
    public static T? Reflection<T>(IConfiguration section) => section.Get<T>();

    /// <summary><c>services.Configure&lt;T&gt;(section)</c> then <see cref="IOptions{TOptions}" />, for the calls written per type below.</summary>
    public static T Options<T>(Action<IServiceCollection> configure) where T : class
    {
        var services = new ServiceCollection();
        configure(services);
        using var provider = services.BuildServiceProvider();
        return provider.GetRequiredService<IOptions<T>>().Value;
    }
}

/// <summary>
///     Microsoft's generated binder. Its generator only intercepts calls whose type is known where they're made, so there's one
///     call per type: <c>Configure&lt;T&gt;</c> for types the options pipeline creates, <c>Get&lt;T&gt;()</c> for the others.
/// </summary>
internal static class MicrosoftBinder
{
    public static EmptyTarget EmptyTarget(IConfiguration s) => Binders.Options<EmptyTarget>(z => z.Configure<EmptyTarget>(s));
    public static NullableTarget NullableTarget(IConfiguration s) => Binders.Options<NullableTarget>(z => z.Configure<NullableTarget>(s));
    public static BothCtorsClass BothCtorsClass(IConfiguration s) => Binders.Options<BothCtorsClass>(z => z.Configure<BothCtorsClass>(s));
    public static GetOnlyCollections GetOnlyCollections(IConfiguration s) => Binders.Options<GetOnlyCollections>(z => z.Configure<GetOnlyCollections>(s));
    public static KeyNameClass KeyNameClass(IConfiguration s) => Binders.Options<KeyNameClass>(z => z.Configure<KeyNameClass>(s));
    public static HasStructs HasStructs(IConfiguration s) => Binders.Options<HasStructs>(z => z.Configure<HasStructs>(s));
    public static HasRecordProp HasRecordProp(IConfiguration s) => Binders.Options<HasRecordProp>(z => z.Configure<HasRecordProp>(s));
    public static HasUncreatable HasUncreatable(IConfiguration s) => Binders.Options<HasUncreatable>(z => z.Configure<HasUncreatable>(s));
    public static Polymorphic Polymorphic(IConfiguration s) => Binders.Options<Polymorphic>(z => z.Configure<Polymorphic>(s));
    public static Inner Inner(IConfiguration s) => Binders.Options<Inner>(z => z.Configure<Inner>(s));
    public static HolderPoco HolderPoco(IConfiguration s) => Binders.Options<HolderPoco>(z => z.Configure<HolderPoco>(s));
    public static HolderString HolderString(IConfiguration s) => Binders.Options<HolderString>(z => z.Configure<HolderString>(s));
    public static HolderInt HolderInt(IConfiguration s) => Binders.Options<HolderInt>(z => z.Configure<HolderInt>(s));
    public static HolderList HolderList(IConfiguration s) => Binders.Options<HolderList>(z => z.Configure<HolderList>(s));
    public static HolderDictInner HolderDictInner(IConfiguration s) => Binders.Options<HolderDictInner>(z => z.Configure<HolderDictInner>(s));
    public static HolderObject HolderObject(IConfiguration s) => Binders.Options<HolderObject>(z => z.Configure<HolderObject>(s));
    public static CollTarget CollTarget(IConfiguration s) => Binders.Options<CollTarget>(z => z.Configure<CollTarget>(s));
    public static CollTargetNull CollTargetNull(IConfiguration s) => Binders.Options<CollTargetNull>(z => z.Configure<CollTargetNull>(s));
    public static ScalarTarget ScalarTarget(IConfiguration s) => Binders.Options<ScalarTarget>(z => z.Configure<ScalarTarget>(s));
    public static UnknownTarget UnknownTarget(IConfiguration s) => Binders.Options<UnknownTarget>(z => z.Configure<UnknownTarget>(s));

    public static PosRecord? PosRecord(IConfiguration s) => s.Get<PosRecord>();
    public static PosRecordDefault? PosRecordDefault(IConfiguration s) => s.Get<PosRecordDefault>();
    public static NullableParamRecord? NullableParamRecord(IConfiguration s) => s.Get<NullableParamRecord>();
    public static SingleCtorClass? SingleCtorClass(IConfiguration s) => s.Get<SingleCtorClass>();
    public static CtorThenSetter? CtorThenSetter(IConfiguration s) => s.Get<CtorThenSetter>();
    public static TransformRecord? TransformRecord(IConfiguration s) => s.Get<TransformRecord>();
    public static RecordWithInitList? RecordWithInitList(IConfiguration s) => s.Get<RecordWithInitList>();
    public static CasingCtor? CasingCtor(IConfiguration s) => s.Get<CasingCtor>();
    public static KeyNameRecord? KeyNameRecord(IConfiguration s) => s.Get<KeyNameRecord>();
}
