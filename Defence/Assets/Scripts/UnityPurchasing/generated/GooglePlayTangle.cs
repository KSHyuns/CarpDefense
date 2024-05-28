// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("EbU+LTf94Aw0xXLTK2auG8YJeEoAY8ukpkgh9yH/gHcDAP2y6cmWeH19MN5VAk+m8OXS79AKTWVFOac28UPA4/HMx8jrR4lHNszAwMDEwcJDwM7B8UPAy8NDwMDBUJEvP7DmwbaZQq01ijJYM3CzHqQeSKaVWmb2DfT5928lL6Fal4HSbvLyjw1/mw4hTZJYjNXRjyLodb7RhzXzd1TlynAZ5VZB5VccupgBQlmITs8f3aHTqcgK0nNWU7Gj6xBV5e9ARpjXimrCEp7l1VrupyAARDEuW6wI6fwk3xt+21ZrE25clZzj/qqn7NycO2rrDHMg/guezGWU+W7RFIEjXxc/l2l1wcpcYTpQfhoDlPUxXl2196zrtWbFx7nmbdYYoMPCwMHA");
        private static int[] order = new int[] { 13,7,11,13,7,10,6,8,11,9,11,12,13,13,14 };
        private static int key = 193;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
