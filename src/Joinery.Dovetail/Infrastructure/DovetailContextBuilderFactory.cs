using PropertiesType = System.Collections.Generic.IDictionary<object, object>;

namespace Joinery.Dovetail.Infrastructure;


/// <summary>
///   A delegate that can be used to create a <see cref="DovetailContextBuilder" />.
/// </summary>
/// <param name="properties">The properties to initialize the context builder with.</param>
/// <param name="categories">The categories of joints the context loads besides the <c>Core</c> joints, which are always loaded. <see langword="null" /> or an empty set loads the <c>Application</c> joints as well.</param>
/// <returns>A new instance of <see cref="DovetailContextBuilder" />.</returns>
public delegate DovetailContextBuilder DovetailContextBuilderFactory(PropertiesType? properties = null, IEnumerable<DovetailCategory>? categories = null);
