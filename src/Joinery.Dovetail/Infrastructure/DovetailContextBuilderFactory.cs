using PropertiesType = System.Collections.Generic.IDictionary<object, object>;

namespace Joinery.Dovetail.Infrastructure;


/// <summary>
///   A delegate that can be used to create a <see cref="DovetailContextBuilder" />.
/// </summary>
/// <param name="properties">The properties to initialize the context builder with.</param>
/// <param name="categories">The categories the context is limited to: only the joints of these categories are loaded, and <see langword="null" /> or an empty set loads every joint.</param>
/// <returns>A new instance of <see cref="DovetailContextBuilder" />.</returns>
public delegate DovetailContextBuilder DovetailContextBuilderFactory(PropertiesType? properties = null, IEnumerable<DovetailCategory>? categories = null);
