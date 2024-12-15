using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace ReadRack.Fronend.Helpers
{
    public static class IJSRuntimeExtensionMethods
    {
        public static ValueTask<object> SetLocalStorage (this IJSRuntime jSRuntime, string key, string content)
        {
            return jSRuntime.InvokeAsync<object>("localStorage.setItem", key, content);
        }
        public static ValueTask<object> GetLocalStorage(this IJSRuntime js, string key)
        {
            return js.InvokeAsync<object>("localStorage.getItem", key);
        }
        public static ValueTask<object> RemoveLocalStorage(this IJSRuntime js, string key)
        {
            return js.InvokeAsync<object>("localStorage.removeItem", key);

        }
    }
}
