/*
 * A set of static helper classes that provide easy runtime access to the games resources.
 * This file is auto-generated. Any changes made to it are lost as soon as Duality decides
 * to regenerate it.
 */
namespace GameRes
{
	public static class Data {
		public static class duality_test_map_map {
			public static Duality.ContentRef<DualityTiled.Core.TmxMap> duality_test_map_TmxMap { get { return Duality.ContentProvider.RequestContent<DualityTiled.Core.TmxMap>(@"Data\duality_test_map.map\duality_test_map.TmxMap.res"); }}
			public static void LoadAll() {
				duality_test_map_TmxMap.MakeAvailable();
			}
		}
		public static Duality.ContentRef<Duality.Resources.Scene> Scene_Scene { get { return Duality.ContentProvider.RequestContent<Duality.Resources.Scene>(@"Data\Scene.Scene.res"); }}
		public static void LoadAll() {
			duality_test_map_map.LoadAll();
			Scene_Scene.MakeAvailable();
		}
	}

}
