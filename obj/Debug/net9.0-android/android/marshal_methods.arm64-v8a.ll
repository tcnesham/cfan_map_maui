; ModuleID = 'marshal_methods.arm64-v8a.ll'
source_filename = "marshal_methods.arm64-v8a.ll"
target datalayout = "e-m:e-i8:8:32-i16:16:32-i64:64-i128:128-n32:64-S128"
target triple = "aarch64-unknown-linux-android21"

%struct.MarshalMethodName = type {
	i64, ; uint64_t id
	ptr ; char* name
}

%struct.MarshalMethodsManagedClass = type {
	i32, ; uint32_t token
	ptr ; MonoClass klass
}

@assembly_image_cache = dso_local local_unnamed_addr global [383 x ptr] zeroinitializer, align 8

; Each entry maps hash of an assembly name to an index into the `assembly_image_cache` array
@assembly_image_cache_hashes = dso_local local_unnamed_addr constant [1149 x i64] [
	i64 u0x0010bf7088f76c5f, ; 0: Google.Cloud.Firestore.V1 => 189
	i64 u0x001e58127c546039, ; 1: lib_System.Globalization.dll.so => 42
	i64 u0x0024d0f62dee05bd, ; 2: Xamarin.KotlinX.Coroutines.Core.dll => 339
	i64 u0x0071cf2d27b7d61e, ; 3: lib_Xamarin.AndroidX.SwipeRefreshLayout.dll.so => 295
	i64 u0x01109b0e4d99e61f, ; 4: System.ComponentModel.Annotations.dll => 13
	i64 u0x020f428300334897, ; 5: Grpc.Net.Client.dll => 197
	i64 u0x02123411c4e01926, ; 6: lib_Xamarin.AndroidX.Navigation.Runtime.dll.so => 285
	i64 u0x022f31be406de945, ; 7: Microsoft.Extensions.Options.ConfigurationExtensions => 213
	i64 u0x0284512fad379f7e, ; 8: System.Runtime.Handles => 105
	i64 u0x02abedc11addc1ed, ; 9: lib_Mono.Android.Runtime.dll.so => 171
	i64 u0x02f55bf70672f5c8, ; 10: lib_System.IO.FileSystem.DriveInfo.dll.so => 48
	i64 u0x032267b2a94db371, ; 11: lib_Xamarin.AndroidX.AppCompat.dll.so => 241
	i64 u0x03621c804933a890, ; 12: System.Buffers => 7
	i64 u0x0366e95fb32b7b8f, ; 13: lib_ISO3166.dll.so => 199
	i64 u0x0399610510a38a38, ; 14: lib_System.Private.DataContractSerialization.dll.so => 86
	i64 u0x043032f1d071fae0, ; 15: ru/Microsoft.Maui.Controls.resources => 368
	i64 u0x044440a55165631e, ; 16: lib-cs-Microsoft.Maui.Controls.resources.dll.so => 346
	i64 u0x046eb1581a80c6b0, ; 17: vi/Microsoft.Maui.Controls.resources => 374
	i64 u0x047408741db2431a, ; 18: Xamarin.AndroidX.DynamicAnimation => 261
	i64 u0x04acae429ea0efac, ; 19: Xamarin.Grpc.Context => 326
	i64 u0x0517ef04e06e9f76, ; 20: System.Net.Primitives => 71
	i64 u0x051a3be159e4ef99, ; 21: Xamarin.GooglePlayServices.Tasks => 323
	i64 u0x0565d18c6da3de38, ; 22: Xamarin.AndroidX.RecyclerView => 288
	i64 u0x0581db89237110e9, ; 23: lib_System.Collections.dll.so => 12
	i64 u0x05989cb940b225a9, ; 24: Microsoft.Maui.dll => 218
	i64 u0x05a1c25e78e22d87, ; 25: lib_System.Runtime.CompilerServices.Unsafe.dll.so => 102
	i64 u0x06076b5d2b581f08, ; 26: zh-HK/Microsoft.Maui.Controls.resources => 375
	i64 u0x06388ffe9f6c161a, ; 27: System.Xml.Linq.dll => 156
	i64 u0x06600c4c124cb358, ; 28: System.Configuration.dll => 19
	i64 u0x066a5fe61c4a5170, ; 29: lib_GoogleApi.dll.so => 193
	i64 u0x067f95c5ddab55b3, ; 30: lib_Xamarin.AndroidX.Fragment.Ktx.dll.so => 266
	i64 u0x0680a433c781bb3d, ; 31: Xamarin.AndroidX.Collection.Jvm => 248
	i64 u0x069fff96ec92a91d, ; 32: System.Xml.XPath.dll => 161
	i64 u0x070b0847e18dab68, ; 33: Xamarin.AndroidX.Emoji2.ViewsHelper.dll => 263
	i64 u0x0739448d84d3b016, ; 34: lib_Xamarin.AndroidX.VectorDrawable.dll.so => 298
	i64 u0x07469f2eecce9e85, ; 35: mscorlib.dll => 167
	i64 u0x07c57877c7ba78ad, ; 36: ru/Microsoft.Maui.Controls.resources.dll => 368
	i64 u0x07dcdc7460a0c5e4, ; 37: System.Collections.NonGeneric => 10
	i64 u0x08122e52765333c8, ; 38: lib_Microsoft.Extensions.Logging.Debug.dll.so => 211
	i64 u0x088610fc2509f69e, ; 39: lib_Xamarin.AndroidX.VectorDrawable.Animated.dll.so => 299
	i64 u0x088b9ffb2d85b417, ; 40: lib_Maui.GoogleMaps.dll.so => 223
	i64 u0x08a7c865576bbde7, ; 41: System.Reflection.Primitives => 96
	i64 u0x08c9d051a4a817e5, ; 42: Xamarin.AndroidX.CustomView.PoolingContainer.dll => 259
	i64 u0x08f3c9788ee2153c, ; 43: Xamarin.AndroidX.DrawerLayout => 260
	i64 u0x09138715c92dba90, ; 44: lib_System.ComponentModel.Annotations.dll.so => 13
	i64 u0x0919c28b89381a0b, ; 45: lib_Microsoft.Extensions.Options.dll.so => 212
	i64 u0x092266563089ae3e, ; 46: lib_System.Collections.NonGeneric.dll.so => 10
	i64 u0x098b50f911ccea8d, ; 47: lib_Xamarin.GooglePlayServices.Basement.dll.so => 321
	i64 u0x09d144a7e214d457, ; 48: System.Security.Cryptography => 127
	i64 u0x09da6dfc3439e851, ; 49: lib_Xamarin.Firebase.Components.dll.so => 307
	i64 u0x09e2b9f743db21a8, ; 50: lib_System.Reflection.Metadata.dll.so => 95
	i64 u0x0abb3e2b271edc45, ; 51: System.Threading.Channels.dll => 140
	i64 u0x0b06b1feab070143, ; 52: System.Formats.Tar => 39
	i64 u0x0b3b632c3bbee20c, ; 53: sk/Microsoft.Maui.Controls.resources => 369
	i64 u0x0b6aff547b84fbe9, ; 54: Xamarin.KotlinX.Serialization.Core.Jvm => 342
	i64 u0x0be2e1f8ce4064ed, ; 55: Xamarin.AndroidX.ViewPager => 301
	i64 u0x0c279376b1ae96ae, ; 56: lib_System.CodeDom.dll.so => 228
	i64 u0x0c3ca6cc978e2aae, ; 57: pt-BR/Microsoft.Maui.Controls.resources => 365
	i64 u0x0c3dd9438f54f672, ; 58: lib_Xamarin.GooglePlayServices.Maps.dll.so => 322
	i64 u0x0c59ad9fbbd43abe, ; 59: Mono.Android => 172
	i64 u0x0c65741e86371ee3, ; 60: lib_Xamarin.Android.Glide.GifDecoder.dll.so => 235
	i64 u0x0c74af560004e816, ; 61: Microsoft.Win32.Registry.dll => 5
	i64 u0x0c7790f60165fc06, ; 62: lib_Microsoft.Maui.Essentials.dll.so => 219
	i64 u0x0c83c82812e96127, ; 63: lib_System.Net.Mail.dll.so => 67
	i64 u0x0cce4bce83380b7f, ; 64: Xamarin.AndroidX.Security.SecurityCrypto => 292
	i64 u0x0d13cd7cce4284e4, ; 65: System.Security.SecureString => 130
	i64 u0x0d3b5ab8b2766190, ; 66: lib_Microsoft.Bcl.AsyncInterfaces.dll.so => 200
	i64 u0x0d565cb22b8879da, ; 67: lib_Grpc.Core.Api.dll.so => 196
	i64 u0x0d63f4f73521c24f, ; 68: lib_Xamarin.AndroidX.SavedState.SavedState.Ktx.dll.so => 291
	i64 u0x0e04e702012f8463, ; 69: Xamarin.AndroidX.Emoji2 => 262
	i64 u0x0e14e73a54dda68e, ; 70: lib_System.Net.NameResolution.dll.so => 68
	i64 u0x0f37dd7a62ae99af, ; 71: lib_Xamarin.AndroidX.Collection.Ktx.dll.so => 249
	i64 u0x0f5e7abaa7cf470a, ; 72: System.Net.HttpListener => 66
	i64 u0x1001f97bbe242e64, ; 73: System.IO.UnmanagedMemoryStream => 57
	i64 u0x102861e4055f511a, ; 74: Microsoft.Bcl.AsyncInterfaces.dll => 200
	i64 u0x102a31b45304b1da, ; 75: Xamarin.AndroidX.CustomView => 258
	i64 u0x1065c4cb554c3d75, ; 76: System.IO.IsolatedStorage.dll => 52
	i64 u0x10f6cfcbcf801616, ; 77: System.IO.Compression.Brotli => 43
	i64 u0x114443cdcf2091f1, ; 78: System.Security.Cryptography.Primitives => 125
	i64 u0x117554f1bd086807, ; 79: Acr.UserDialogs.dll => 174
	i64 u0x11a603952763e1d4, ; 80: System.Net.Mail => 67
	i64 u0x11a70d0e1009fb11, ; 81: System.Net.WebSockets.dll => 81
	i64 u0x11f26371eee0d3c1, ; 82: lib_Xamarin.AndroidX.Lifecycle.Runtime.Ktx.dll.so => 276
	i64 u0x11fbe62d469cc1c8, ; 83: Microsoft.VisualStudio.DesignTools.TapContract.dll => 380
	i64 u0x12128b3f59302d47, ; 84: lib_System.Xml.Serialization.dll.so => 158
	i64 u0x123639456fb056da, ; 85: System.Reflection.Emit.Lightweight.dll => 92
	i64 u0x124b1cd9ce23ae6f, ; 86: Google.Api.Gax.Rest => 182
	i64 u0x12521e9764603eaa, ; 87: lib_System.Resources.Reader.dll.so => 99
	i64 u0x125b7f94acb989db, ; 88: Xamarin.AndroidX.RecyclerView.dll => 288
	i64 u0x12d3b63863d4ab0b, ; 89: lib_System.Threading.Overlapped.dll.so => 141
	i64 u0x12f23aabd624cf79, ; 90: lib_Google.Cloud.Firestore.V1.dll.so => 189
	i64 u0x134eab1061c395ee, ; 91: System.Transactions => 151
	i64 u0x138567fa954faa55, ; 92: Xamarin.AndroidX.Browser => 245
	i64 u0x13a01de0cbc3f06c, ; 93: lib-fr-Microsoft.Maui.Controls.resources.dll.so => 352
	i64 u0x13beedefb0e28a45, ; 94: lib_System.Xml.XmlDocument.dll.so => 162
	i64 u0x13f1e5e209e91af4, ; 95: lib_Java.Interop.dll.so => 169
	i64 u0x13f1e880c25d96d1, ; 96: he/Microsoft.Maui.Controls.resources => 353
	i64 u0x143d8ea60a6a4011, ; 97: Microsoft.Extensions.DependencyInjection.Abstractions => 205
	i64 u0x1497051b917530bd, ; 98: lib_System.Net.WebSockets.dll.so => 81
	i64 u0x14b78ce3adce0011, ; 99: Microsoft.VisualStudio.DesignTools.TapContract => 380
	i64 u0x14d612a531c79c05, ; 100: Xamarin.JSpecify.dll => 334
	i64 u0x14e68447938213b7, ; 101: Xamarin.AndroidX.Collection.Ktx.dll => 249
	i64 u0x152a448bd1e745a7, ; 102: Microsoft.Win32.Primitives => 4
	i64 u0x1557de0138c445f4, ; 103: lib_Microsoft.Win32.Registry.dll.so => 5
	i64 u0x15bdc156ed462f2f, ; 104: lib_System.IO.FileSystem.dll.so => 51
	i64 u0x15e300c2c1668655, ; 105: System.Resources.Writer.dll => 101
	i64 u0x16726eac78495e6d, ; 106: Xamarin.Grpc.Stub => 330
	i64 u0x16bf2a22df043a09, ; 107: System.IO.Pipes.dll => 56
	i64 u0x16c9d17b90a80fc1, ; 108: lib_Xamarin.Io.OpenCensus.OpenCensusApi.dll.so => 331
	i64 u0x16ea2b318ad2d830, ; 109: System.Security.Cryptography.Algorithms => 120
	i64 u0x16eeae54c7ebcc08, ; 110: System.Reflection.dll => 98
	i64 u0x17125c9a85b4929f, ; 111: lib_netstandard.dll.so => 168
	i64 u0x1716866f7416792e, ; 112: lib_System.Security.AccessControl.dll.so => 118
	i64 u0x174f71c46216e44a, ; 113: Xamarin.KotlinX.Coroutines.Core => 339
	i64 u0x1752c12f1e1fc00c, ; 114: System.Core => 21
	i64 u0x1784ffe4a29e620e, ; 115: lib_Google.Apis.Bigquery.v2.dll.so => 185
	i64 u0x17b56e25558a5d36, ; 116: lib-hu-Microsoft.Maui.Controls.resources.dll.so => 356
	i64 u0x17f9358913beb16a, ; 117: System.Text.Encodings.Web => 137
	i64 u0x1809fb23f29ba44a, ; 118: lib_System.Reflection.TypeExtensions.dll.so => 97
	i64 u0x18402a709e357f3b, ; 119: lib_Xamarin.KotlinX.Serialization.Core.Jvm.dll.so => 342
	i64 u0x18a9befae51bb361, ; 120: System.Net.WebClient => 77
	i64 u0x18f0ce884e87d89a, ; 121: nb/Microsoft.Maui.Controls.resources.dll => 362
	i64 u0x19777fba3c41b398, ; 122: Xamarin.AndroidX.Startup.StartupRuntime.dll => 294
	i64 u0x19a4c090f14ebb66, ; 123: System.Security.Claims => 119
	i64 u0x19d70854413e7c06, ; 124: lib_OpenLocationCode.dll.so => 224
	i64 u0x1a539258f88190d6, ; 125: lib_System.Linq.Async.dll.so => 229
	i64 u0x1a91866a319e9259, ; 126: lib_System.Collections.Concurrent.dll.so => 8
	i64 u0x1aac34d1917ba5d3, ; 127: lib_System.dll.so => 165
	i64 u0x1aad60783ffa3e5b, ; 128: lib-th-Microsoft.Maui.Controls.resources.dll.so => 371
	i64 u0x1aea8f1c3b282172, ; 129: lib_System.Net.Ping.dll.so => 70
	i64 u0x1b4b7a1d0d265fa2, ; 130: Xamarin.Android.Glide.DiskLruCache => 234
	i64 u0x1bbdb16cfa73e785, ; 131: Xamarin.AndroidX.Lifecycle.Runtime.Ktx.Android => 277
	i64 u0x1bc766e07b2b4241, ; 132: Xamarin.AndroidX.ResourceInspection.Annotation.dll => 289
	i64 u0x1c292b1598348d77, ; 133: Microsoft.Extensions.Diagnostics.dll => 206
	i64 u0x1c753b5ff15bce1b, ; 134: Mono.Android.Runtime.dll => 171
	i64 u0x1cb6a0ededc839e2, ; 135: lib_Google.Apis.Auth.dll.so => 184
	i64 u0x1cd47467799d8250, ; 136: System.Threading.Tasks.dll => 145
	i64 u0x1d23eafdc6dc346c, ; 137: System.Globalization.Calendars.dll => 40
	i64 u0x1da4110562816681, ; 138: Xamarin.AndroidX.Security.SecurityCrypto.dll => 292
	i64 u0x1db6820994506bf5, ; 139: System.IO.FileSystem.AccessControl.dll => 47
	i64 u0x1dba6509cc55b56f, ; 140: lib_Google.Protobuf.dll.so => 192
	i64 u0x1dbb0c2c6a999acb, ; 141: System.Diagnostics.StackTrace => 30
	i64 u0x1dcda680b17dc5bb, ; 142: lib_Xamarin.Google.Guava.FailureAccess.dll.so => 318
	i64 u0x1e3d87657e9659bc, ; 143: Xamarin.AndroidX.Navigation.UI => 286
	i64 u0x1e5c77a1f58ff0f8, ; 144: GeoJSON.Net => 178
	i64 u0x1e71143913d56c10, ; 145: lib-ko-Microsoft.Maui.Controls.resources.dll.so => 360
	i64 u0x1e7c31185e2fb266, ; 146: lib_System.Threading.Tasks.Parallel.dll.so => 144
	i64 u0x1ed8fcce5e9b50a0, ; 147: Microsoft.Extensions.Options.dll => 212
	i64 u0x1f055d15d807e1b2, ; 148: System.Xml.XmlSerializer => 163
	i64 u0x1f1ed22c1085f044, ; 149: lib_System.Diagnostics.FileVersionInfo.dll.so => 28
	i64 u0x1f61df9c5b94d2c1, ; 150: lib_System.Numerics.dll.so => 84
	i64 u0x1f750bb5421397de, ; 151: lib_Xamarin.AndroidX.Tracing.Tracing.dll.so => 296
	i64 u0x20237ea48006d7a8, ; 152: lib_System.Net.WebClient.dll.so => 77
	i64 u0x209375905fcc1bad, ; 153: lib_System.IO.Compression.Brotli.dll.so => 43
	i64 u0x20e085517023eec8, ; 154: lib_Google.Api.Gax.dll.so => 180
	i64 u0x20fab3cf2dfbc8df, ; 155: lib_System.Diagnostics.Process.dll.so => 29
	i64 u0x2110167c128cba15, ; 156: System.Globalization => 42
	i64 u0x21419508838f7547, ; 157: System.Runtime.CompilerServices.VisualC => 103
	i64 u0x2174319c0d835bc9, ; 158: System.Runtime => 117
	i64 u0x218ae22aa3ec33e7, ; 159: Xamarin.Grpc.Protobuf.Lite.dll => 329
	i64 u0x2198e5bc8b7153fa, ; 160: Xamarin.AndroidX.Annotation.Experimental.dll => 239
	i64 u0x219ea1b751a4dee4, ; 161: lib_System.IO.Compression.ZipFile.dll.so => 45
	i64 u0x21cc7e445dcd5469, ; 162: System.Reflection.Emit.ILGeneration => 91
	i64 u0x220fd4f2e7c48170, ; 163: th/Microsoft.Maui.Controls.resources => 371
	i64 u0x224538d85ed15a82, ; 164: System.IO.Pipes => 56
	i64 u0x22908438c6bed1af, ; 165: lib_System.Threading.Timer.dll.so => 148
	i64 u0x22fbc14e981e3b45, ; 166: lib_Microsoft.VisualStudio.DesignTools.MobileTapContracts.dll.so => 379
	i64 u0x2347c268e3e4e536, ; 167: Xamarin.GooglePlayServices.Basement.dll => 321
	i64 u0x237be844f1f812c7, ; 168: System.Threading.Thread.dll => 146
	i64 u0x23852b3bdc9f7096, ; 169: System.Resources.ResourceManager => 100
	i64 u0x23986dd7e5d4fc01, ; 170: System.IO.FileSystem.Primitives.dll => 49
	i64 u0x23b0dd507a933aa9, ; 171: Google.Api.Gax => 180
	i64 u0x2407aef2bbe8fadf, ; 172: System.Console => 20
	i64 u0x240abe014b27e7d3, ; 173: Xamarin.AndroidX.Core.dll => 254
	i64 u0x247619fe4413f8bf, ; 174: System.Runtime.Serialization.Primitives.dll => 114
	i64 u0x24b87318591adabe, ; 175: lib_Xamarin.Firebase.Database.Collection.dll.so => 308
	i64 u0x24b95d581a70fbee, ; 176: Grpc.Auth.dll => 195
	i64 u0x24d4238047d7310f, ; 177: Google.Apis.Auth => 184
	i64 u0x24de8d301281575e, ; 178: Xamarin.Android.Glide => 232
	i64 u0x252073cc3caa62c2, ; 179: fr/Microsoft.Maui.Controls.resources.dll => 352
	i64 u0x256b8d41255f01b1, ; 180: Xamarin.Google.Crypto.Tink.Android => 314
	i64 u0x2662c629b96b0b30, ; 181: lib_Xamarin.Kotlin.StdLib.dll.so => 335
	i64 u0x268c1439f13bcc29, ; 182: lib_Microsoft.Extensions.Primitives.dll.so => 214
	i64 u0x26918e5f13c8fc7c, ; 183: Xamarin.Firebase.Firestore => 309
	i64 u0x26a670e154a9c54b, ; 184: System.Reflection.Extensions.dll => 94
	i64 u0x26d077d9678fe34f, ; 185: System.IO.dll => 58
	i64 u0x273f3515de5faf0d, ; 186: id/Microsoft.Maui.Controls.resources.dll => 357
	i64 u0x2742545f9094896d, ; 187: hr/Microsoft.Maui.Controls.resources => 355
	i64 u0x2759af78ab94d39b, ; 188: System.Net.WebSockets => 81
	i64 u0x27b2b16f3e9de038, ; 189: Xamarin.Google.Crypto.Tink.Android.dll => 314
	i64 u0x27b410442fad6cf1, ; 190: Java.Interop.dll => 169
	i64 u0x27b97e0d52c3034a, ; 191: System.Diagnostics.Debug => 26
	i64 u0x27d88445c936a1af, ; 192: lib_Xamarin.Grpc.Android.dll.so => 324
	i64 u0x2801845a2c71fbfb, ; 193: System.Net.Primitives.dll => 71
	i64 u0x286835e259162700, ; 194: lib_Xamarin.AndroidX.ProfileInstaller.ProfileInstaller.dll.so => 287
	i64 u0x28e52865585a1ebe, ; 195: Microsoft.Extensions.Diagnostics.Abstractions.dll => 207
	i64 u0x2949f3617a02c6b2, ; 196: Xamarin.AndroidX.ExifInterface => 264
	i64 u0x29f947844fb7fc11, ; 197: Microsoft.Maui.Controls.HotReload.Forms => 378
	i64 u0x2a128783efe70ba0, ; 198: uk/Microsoft.Maui.Controls.resources.dll => 373
	i64 u0x2a3b095612184159, ; 199: lib_System.Net.NetworkInformation.dll.so => 69
	i64 u0x2a6507a5ffabdf28, ; 200: System.Diagnostics.TraceSource.dll => 33
	i64 u0x2ad156c8e1354139, ; 201: fi/Microsoft.Maui.Controls.resources => 351
	i64 u0x2ad5d6b13b7a3e04, ; 202: System.ComponentModel.DataAnnotations.dll => 14
	i64 u0x2af298f63581d886, ; 203: System.Text.RegularExpressions.dll => 139
	i64 u0x2afc1c4f898552ee, ; 204: lib_System.Formats.Asn1.dll.so => 38
	i64 u0x2b0f316f4c87d83a, ; 205: Xamarin.Io.OpenCensus.OpenCensusApi => 331
	i64 u0x2b148910ed40fbf9, ; 206: zh-Hant/Microsoft.Maui.Controls.resources.dll => 377
	i64 u0x2b6989d78cba9a15, ; 207: Xamarin.AndroidX.Concurrent.Futures.dll => 250
	i64 u0x2c8bd14bb93a7d82, ; 208: lib-pl-Microsoft.Maui.Controls.resources.dll.so => 364
	i64 u0x2cbd9262ca785540, ; 209: lib_System.Text.Encoding.CodePages.dll.so => 134
	i64 u0x2cc9e1fed6257257, ; 210: lib_System.Reflection.Emit.Lightweight.dll.so => 92
	i64 u0x2cd723e9fe623c7c, ; 211: lib_System.Private.Xml.Linq.dll.so => 88
	i64 u0x2ce03196fe1170d2, ; 212: Microsoft.Maui.Controls.Maps.dll => 216
	i64 u0x2d169d318a968379, ; 213: System.Threading.dll => 149
	i64 u0x2d1d1413dd10c597, ; 214: Xamarin.Google.Guava.FailureAccess => 318
	i64 u0x2d47774b7d993f59, ; 215: sv/Microsoft.Maui.Controls.resources.dll => 370
	i64 u0x2d5ffcae1ad0aaca, ; 216: System.Data.dll => 24
	i64 u0x2d6267ac7de1d619, ; 217: Xamarin.Firebase.Database.Collection.dll => 308
	i64 u0x2db915caf23548d2, ; 218: System.Text.Json.dll => 138
	i64 u0x2dcaa0bb15a4117a, ; 219: System.IO.UnmanagedMemoryStream.dll => 57
	i64 u0x2e5a40c319acb800, ; 220: System.IO.FileSystem => 51
	i64 u0x2e6f1f226821322a, ; 221: el/Microsoft.Maui.Controls.resources.dll => 349
	i64 u0x2ef38b95dd888c55, ; 222: lib_CFAN.SchoolMap.Maui.dll.so => 0
	i64 u0x2f02f94df3200fe5, ; 223: System.Diagnostics.Process => 29
	i64 u0x2f2e98e1c89b1aff, ; 224: System.Xml.ReaderWriter => 157
	i64 u0x2f5911d9ba814e4e, ; 225: System.Diagnostics.Tracing => 34
	i64 u0x2f84070a459bc31f, ; 226: lib_System.Xml.dll.so => 164
	i64 u0x2fd92a71c7638cfd, ; 227: Xamarin.Firebase.Database.Collection => 308
	i64 u0x2ff49de6a71764a1, ; 228: lib_Microsoft.Extensions.Http.dll.so => 208
	i64 u0x309ee9eeec09a71e, ; 229: lib_Xamarin.AndroidX.Fragment.dll.so => 265
	i64 u0x30bde19041cd89dd, ; 230: lib_Microsoft.Maui.Maps.dll.so => 221
	i64 u0x30c6dda129408828, ; 231: System.IO.IsolatedStorage => 52
	i64 u0x31195fef5d8fb552, ; 232: _Microsoft.Android.Resource.Designer.dll => 382
	i64 u0x312c8ed623cbfc8d, ; 233: Xamarin.AndroidX.Window.dll => 303
	i64 u0x31496b779ed0663d, ; 234: lib_System.Reflection.DispatchProxy.dll.so => 90
	i64 u0x315f08d19390dc36, ; 235: Xamarin.Google.ErrorProne.TypeAnnotations => 316
	i64 u0x32243413e774362a, ; 236: Xamarin.AndroidX.CardView.dll => 246
	i64 u0x3235427f8d12dae1, ; 237: lib_System.Drawing.Primitives.dll.so => 35
	i64 u0x329753a17a517811, ; 238: fr/Microsoft.Maui.Controls.resources => 352
	i64 u0x32aa989ff07a84ff, ; 239: lib_System.Xml.ReaderWriter.dll.so => 157
	i64 u0x32b975ac62e778ea, ; 240: lib_AndHUD.dll.so => 176
	i64 u0x33829542f112d59b, ; 241: System.Collections.Immutable => 9
	i64 u0x33a31443733849fe, ; 242: lib-es-Microsoft.Maui.Controls.resources.dll.so => 350
	i64 u0x33ec63a7e226adfb, ; 243: Google.Cloud.Location.dll => 190
	i64 u0x341abc357fbb4ebf, ; 244: lib_System.Net.Sockets.dll.so => 76
	i64 u0x342397b849d48e49, ; 245: Xamarin.Grpc.Core => 327
	i64 u0x3496c1e2dcaf5ecc, ; 246: lib_System.IO.Pipes.AccessControl.dll.so => 55
	i64 u0x34dfd74fe2afcf37, ; 247: Microsoft.Maui => 218
	i64 u0x34e292762d9615df, ; 248: cs/Microsoft.Maui.Controls.resources.dll => 346
	i64 u0x35080e71f38b333d, ; 249: Xamarin.Protobuf.Lite => 343
	i64 u0x3508234247f48404, ; 250: Microsoft.Maui.Controls => 215
	i64 u0x353590da528c9d22, ; 251: System.ComponentModel.Annotations => 13
	i64 u0x3549870798b4cd30, ; 252: lib_Xamarin.AndroidX.ViewPager2.dll.so => 302
	i64 u0x355282fc1c909694, ; 253: Microsoft.Extensions.Configuration => 201
	i64 u0x3552fc5d578f0fbf, ; 254: Xamarin.AndroidX.Arch.Core.Common => 243
	i64 u0x355c649948d55d97, ; 255: lib_System.Runtime.Intrinsics.dll.so => 109
	i64 u0x356fd122ba041cb4, ; 256: lib_Xamarin.Grpc.Protobuf.Lite.dll.so => 329
	i64 u0x35ea9d1c6834bc8c, ; 257: Xamarin.AndroidX.Lifecycle.ViewModel.Ktx.dll => 280
	i64 u0x3628ab68db23a01a, ; 258: lib_System.Diagnostics.Tools.dll.so => 32
	i64 u0x364703ab05867b92, ; 259: Xamarin.Firebase.Components => 307
	i64 u0x3673b042508f5b6b, ; 260: lib_System.Runtime.Extensions.dll.so => 104
	i64 u0x36740f1a8ecdc6c4, ; 261: System.Numerics => 84
	i64 u0x36b2b50fdf589ae2, ; 262: System.Reflection.Emit.Lightweight => 92
	i64 u0x36cada77dc79928b, ; 263: System.IO.MemoryMappedFiles => 53
	i64 u0x374ef46b06791af6, ; 264: System.Reflection.Primitives.dll => 96
	i64 u0x376bf93e521a5417, ; 265: lib_Xamarin.Jetbrains.Annotations.dll.so => 333
	i64 u0x379e6c338e5508ad, ; 266: lib_Google.Api.Gax.Grpc.dll.so => 181
	i64 u0x37bc29f3183003b6, ; 267: lib_System.IO.dll.so => 58
	i64 u0x380134e03b1e160a, ; 268: System.Collections.Immutable.dll => 9
	i64 u0x38049b5c59b39324, ; 269: System.Runtime.CompilerServices.Unsafe => 102
	i64 u0x385c17636bb6fe6e, ; 270: Xamarin.AndroidX.CustomView.dll => 258
	i64 u0x38869c811d74050e, ; 271: System.Net.NameResolution.dll => 68
	i64 u0x393c226616977fdb, ; 272: lib_Xamarin.AndroidX.ViewPager.dll.so => 301
	i64 u0x395b3053dde89e41, ; 273: lib_System.Reactive.dll.so => 231
	i64 u0x395e37c3334cf82a, ; 274: lib-ca-Microsoft.Maui.Controls.resources.dll.so => 345
	i64 u0x39a87563fdb248a0, ; 275: System.Reactive.dll => 231
	i64 u0x39aa39fda111d9d3, ; 276: Newtonsoft.Json => 222
	i64 u0x3ab5859054645f72, ; 277: System.Security.Cryptography.Primitives.dll => 125
	i64 u0x3ad75090c3fac0e9, ; 278: lib_Xamarin.AndroidX.ResourceInspection.Annotation.dll.so => 289
	i64 u0x3ae44ac43a1fbdbb, ; 279: System.Runtime.Serialization => 116
	i64 u0x3b860f9932505633, ; 280: lib_System.Text.Encoding.Extensions.dll.so => 135
	i64 u0x3c3aafb6b3a00bf6, ; 281: lib_System.Security.Cryptography.X509Certificates.dll.so => 126
	i64 u0x3c4049146b59aa90, ; 282: System.Runtime.InteropServices.JavaScript => 106
	i64 u0x3c51334447dec9e7, ; 283: Google.LongRunning => 191
	i64 u0x3c7c495f58ac5ee9, ; 284: Xamarin.Kotlin.StdLib => 335
	i64 u0x3c7e5ed3d5db71bb, ; 285: System.Security => 131
	i64 u0x3c90a7b70f45292a, ; 286: Xamarin.Grpc.OkHttp.dll => 328
	i64 u0x3cc1676a8781bdbc, ; 287: Xamarin.Firebase.Auth.Interop.dll => 305
	i64 u0x3cd9d281d402eb9b, ; 288: Xamarin.AndroidX.Browser.dll => 245
	i64 u0x3d1c50cc001a991e, ; 289: Xamarin.Google.Guava.ListenableFuture.dll => 319
	i64 u0x3d2b1913edfc08d7, ; 290: lib_System.Threading.ThreadPool.dll.so => 147
	i64 u0x3d46f0b995082740, ; 291: System.Xml.Linq => 156
	i64 u0x3d8a8f400514a790, ; 292: Xamarin.AndroidX.Fragment.Ktx.dll => 266
	i64 u0x3d9c2a242b040a50, ; 293: lib_Xamarin.AndroidX.Core.dll.so => 254
	i64 u0x3daa14724d8f58e8, ; 294: Google.Protobuf.dll => 192
	i64 u0x3dbb6b9f5ab90fa7, ; 295: lib_Xamarin.AndroidX.DynamicAnimation.dll.so => 261
	i64 u0x3e027e6e728d7f1c, ; 296: Google.LongRunning.dll => 191
	i64 u0x3e5441657549b213, ; 297: Xamarin.AndroidX.ResourceInspection.Annotation => 289
	i64 u0x3e57d4d195c53c2e, ; 298: System.Reflection.TypeExtensions => 97
	i64 u0x3e616ab4ed1f3f15, ; 299: lib_System.Data.dll.so => 24
	i64 u0x3f1d226e6e06db7e, ; 300: Xamarin.AndroidX.SlidingPaneLayout.dll => 293
	i64 u0x3f510adf788828dd, ; 301: System.Threading.Tasks.Extensions => 143
	i64 u0x407a10bb4bf95829, ; 302: lib_Xamarin.AndroidX.Navigation.Common.dll.so => 283
	i64 u0x40c98b6bd77346d4, ; 303: Microsoft.VisualBasic.dll => 3
	i64 u0x41406d6f37320d99, ; 304: Google.Api.Gax.Grpc.dll => 181
	i64 u0x41833cf766d27d96, ; 305: mscorlib => 167
	i64 u0x41cab042be111c34, ; 306: lib_Xamarin.AndroidX.AppCompat.AppCompatResources.dll.so => 242
	i64 u0x423a9ecc4d905a88, ; 307: lib_System.Resources.ResourceManager.dll.so => 100
	i64 u0x423bf51ae7def810, ; 308: System.Xml.XPath => 161
	i64 u0x42418aba44539ffd, ; 309: Google.Cloud.Firestore => 188
	i64 u0x42462ff15ddba223, ; 310: System.Resources.Reader.dll => 99
	i64 u0x4266c67fd9a4ee79, ; 311: Google.Api.CommonProtos => 179
	i64 u0x4291015ff4e5ef71, ; 312: Xamarin.AndroidX.Core.ViewTree.dll => 256
	i64 u0x42a31b86e6ccc3f0, ; 313: System.Diagnostics.Contracts => 25
	i64 u0x42d3cd7add035099, ; 314: System.Management.dll => 230
	i64 u0x430e95b891249788, ; 315: lib_System.Reflection.Emit.dll.so => 93
	i64 u0x43375950ec7c1b6a, ; 316: netstandard.dll => 168
	i64 u0x434c4e1d9284cdae, ; 317: Mono.Android.dll => 172
	i64 u0x43505013578652a0, ; 318: lib_Xamarin.AndroidX.Activity.Ktx.dll.so => 237
	i64 u0x437d06c381ed575a, ; 319: lib_Microsoft.VisualBasic.dll.so => 3
	i64 u0x43950f84de7cc79a, ; 320: pl/Microsoft.Maui.Controls.resources.dll => 364
	i64 u0x43e8ca5bc927ff37, ; 321: lib_Xamarin.AndroidX.Emoji2.ViewsHelper.dll.so => 263
	i64 u0x448bd33429269b19, ; 322: Microsoft.CSharp => 1
	i64 u0x4499fa3c8e494654, ; 323: lib_System.Runtime.Serialization.Primitives.dll.so => 114
	i64 u0x4515080865a951a5, ; 324: Xamarin.Kotlin.StdLib.dll => 335
	i64 u0x4545802489b736b9, ; 325: Xamarin.AndroidX.Fragment.Ktx => 266
	i64 u0x454b4d1e66bb783c, ; 326: Xamarin.AndroidX.Lifecycle.Process => 273
	i64 u0x45b31d67ff6f2b8a, ; 327: lib_Google.Apis.dll.so => 183
	i64 u0x45c40276a42e283e, ; 328: System.Diagnostics.TraceSource => 33
	i64 u0x45d443f2a29adc37, ; 329: System.AppContext.dll => 6
	i64 u0x46a4213bc97fe5ae, ; 330: lib-ru-Microsoft.Maui.Controls.resources.dll.so => 368
	i64 u0x46f84b5cc9b7d78b, ; 331: Xamarin.Io.OpenCensus.OpenCensusContribGrpcMetrics.dll => 332
	i64 u0x47358bd471172e1d, ; 332: lib_System.Xml.Linq.dll.so => 156
	i64 u0x4747e19ad6a1d4bb, ; 333: Grpc.Net.Common => 198
	i64 u0x477d591f9fe3587b, ; 334: Maui.GoogleMaps => 223
	i64 u0x47daf4e1afbada10, ; 335: pt/Microsoft.Maui.Controls.resources => 366
	i64 u0x480c0a47dd42dd81, ; 336: lib_System.IO.MemoryMappedFiles.dll.so => 53
	i64 u0x49e952f19a4e2022, ; 337: System.ObjectModel => 85
	i64 u0x49f6ab815e178ca9, ; 338: lib_Xamarin.Firebase.Common.dll.so => 306
	i64 u0x49f9e6948a8131e4, ; 339: lib_Xamarin.AndroidX.VersionedParcelable.dll.so => 300
	i64 u0x4a0fc182a3c7fc42, ; 340: Xamarin.Protobuf.Lite.dll => 343
	i64 u0x4a5667b2462a664b, ; 341: lib_Xamarin.AndroidX.Navigation.UI.dll.so => 286
	i64 u0x4a7a18981dbd56bc, ; 342: System.IO.Compression.FileSystem.dll => 44
	i64 u0x4aa5c60350917c06, ; 343: lib_Xamarin.AndroidX.Lifecycle.LiveData.Core.Ktx.dll.so => 272
	i64 u0x4b07a0ed0ab33ff4, ; 344: System.Runtime.Extensions.dll => 104
	i64 u0x4b576d47ac054f3c, ; 345: System.IO.FileSystem.AccessControl => 47
	i64 u0x4b7b6532ded934b7, ; 346: System.Text.Json => 138
	i64 u0x4c7755cf07ad2d5f, ; 347: System.Net.Http.Json.dll => 64
	i64 u0x4c9caee94c082049, ; 348: Xamarin.GooglePlayServices.Maps => 322
	i64 u0x4cc5f15266470798, ; 349: lib_Xamarin.AndroidX.Loader.dll.so => 282
	i64 u0x4cf6f67dc77aacd2, ; 350: System.Net.NetworkInformation.dll => 69
	i64 u0x4d3183dd245425d4, ; 351: System.Net.WebSockets.Client.dll => 80
	i64 u0x4d3711d4edd16f99, ; 352: Google.Api.Gax.Rest.dll => 182
	i64 u0x4d479f968a05e504, ; 353: System.Linq.Expressions.dll => 59
	i64 u0x4d55a010ffc4faff, ; 354: System.Private.Xml => 89
	i64 u0x4d5cbe77561c5b2e, ; 355: System.Web.dll => 154
	i64 u0x4d77512dbd86ee4c, ; 356: lib_Xamarin.AndroidX.Arch.Core.Common.dll.so => 243
	i64 u0x4d7793536e79c309, ; 357: System.ServiceProcess => 133
	i64 u0x4d95fccc1f67c7ca, ; 358: System.Runtime.Loader.dll => 110
	i64 u0x4dcf44c3c9b076a2, ; 359: it/Microsoft.Maui.Controls.resources.dll => 358
	i64 u0x4dd9247f1d2c3235, ; 360: Xamarin.AndroidX.Loader.dll => 282
	i64 u0x4e2aeee78e2c4a87, ; 361: Xamarin.AndroidX.ProfileInstaller.ProfileInstaller => 287
	i64 u0x4e32f00cb0937401, ; 362: Mono.Android.Runtime => 171
	i64 u0x4e5eea4668ac2b18, ; 363: System.Text.Encoding.CodePages => 134
	i64 u0x4ebd0c4b82c5eefc, ; 364: lib_System.Threading.Channels.dll.so => 140
	i64 u0x4ee8eaa9c9c1151a, ; 365: System.Globalization.Calendars => 40
	i64 u0x4f21ee6ef9eb527e, ; 366: ca/Microsoft.Maui.Controls.resources => 345
	i64 u0x4fdc964ec1888e25, ; 367: lib_Microsoft.Extensions.Configuration.Binder.dll.so => 203
	i64 u0x4ff8ea8951a69b9f, ; 368: Xamarin.Grpc.Android.dll => 324
	i64 u0x5037f0be3c28c7a3, ; 369: lib_Microsoft.Maui.Controls.dll.so => 215
	i64 u0x506203448c473a65, ; 370: Xamarin.Google.AutoValue.Annotations => 312
	i64 u0x508c1fa6b57728d9, ; 371: Grpc.Net.Common.dll => 198
	i64 u0x50c3a29b21050d45, ; 372: System.Linq.Parallel.dll => 60
	i64 u0x5112ed116d87baf8, ; 373: CommunityToolkit.Mvvm => 177
	i64 u0x5116b21580ae6eb0, ; 374: Microsoft.Extensions.Configuration.Binder.dll => 203
	i64 u0x5131bbe80989093f, ; 375: Xamarin.AndroidX.Lifecycle.ViewModel.Android.dll => 279
	i64 u0x515d61d6527dac70, ; 376: lib_Xamarin.Firebase.Auth.Interop.dll.so => 305
	i64 u0x516324a5050a7e3c, ; 377: System.Net.WebProxy => 79
	i64 u0x516d6f0b21a303de, ; 378: lib_System.Diagnostics.Contracts.dll.so => 25
	i64 u0x51bb8a2afe774e32, ; 379: System.Drawing => 36
	i64 u0x5247c5c32a4140f0, ; 380: System.Resources.Reader => 99
	i64 u0x526bb15e3c386364, ; 381: Xamarin.AndroidX.Lifecycle.Runtime.Ktx.dll => 276
	i64 u0x526ce79eb8e90527, ; 382: lib_System.Net.Primitives.dll.so => 71
	i64 u0x5277169428c6ebf6, ; 383: lib_Grpc.Net.Common.dll.so => 198
	i64 u0x52829f00b4467c38, ; 384: lib_System.Data.Common.dll.so => 22
	i64 u0x529ffe06f39ab8db, ; 385: Xamarin.AndroidX.Core => 254
	i64 u0x52ff996554dbf352, ; 386: Microsoft.Maui.Graphics => 220
	i64 u0x535f7e40e8fef8af, ; 387: lib-sk-Microsoft.Maui.Controls.resources.dll.so => 369
	i64 u0x53978aac584c666e, ; 388: lib_System.Security.Cryptography.Cng.dll.so => 121
	i64 u0x53a96d5c86c9e194, ; 389: System.Net.NetworkInformation => 69
	i64 u0x53be1038a61e8d44, ; 390: System.Runtime.InteropServices.RuntimeInformation.dll => 107
	i64 u0x53c3014b9437e684, ; 391: lib-zh-HK-Microsoft.Maui.Controls.resources.dll.so => 375
	i64 u0x5435e6f049e9bc37, ; 392: System.Security.Claims.dll => 119
	i64 u0x54795225dd1587af, ; 393: lib_System.Runtime.dll.so => 117
	i64 u0x547a34f14e5f6210, ; 394: Xamarin.AndroidX.Lifecycle.Common.dll => 268
	i64 u0x54b42cc2b8e65a84, ; 395: Google.Apis.Core.dll => 186
	i64 u0x556e8b63b660ab8b, ; 396: Xamarin.AndroidX.Lifecycle.Common.Jvm.dll => 269
	i64 u0x5588627c9a108ec9, ; 397: System.Collections.Specialized => 11
	i64 u0x55a898e4f42e3fae, ; 398: Microsoft.VisualBasic.Core.dll => 2
	i64 u0x55fa0c610fe93bb1, ; 399: lib_System.Security.Cryptography.OpenSsl.dll.so => 124
	i64 u0x56442b99bc64bb47, ; 400: System.Runtime.Serialization.Xml.dll => 115
	i64 u0x56a8b26e1aeae27b, ; 401: System.Threading.Tasks.Dataflow => 142
	i64 u0x56f932d61e93c07f, ; 402: System.Globalization.Extensions => 41
	i64 u0x571c5cfbec5ae8e2, ; 403: System.Private.Uri => 87
	i64 u0x57201164aeb974e3, ; 404: Xamarin.Google.Guava.FailureAccess.dll => 318
	i64 u0x576499c9f52fea31, ; 405: Xamarin.AndroidX.Annotation => 238
	i64 u0x579a06fed6eec900, ; 406: System.Private.CoreLib.dll => 173
	i64 u0x57c542c14049b66d, ; 407: System.Diagnostics.DiagnosticSource => 27
	i64 u0x581a8bd5cfda563e, ; 408: System.Threading.Timer => 148
	i64 u0x584ac38e21d2fde1, ; 409: Microsoft.Extensions.Configuration.Binder => 203
	i64 u0x58601b2dda4a27b9, ; 410: lib-ja-Microsoft.Maui.Controls.resources.dll.so => 359
	i64 u0x58688d9af496b168, ; 411: Microsoft.Extensions.DependencyInjection.dll => 204
	i64 u0x588c167a79db6bfb, ; 412: lib_Xamarin.Google.ErrorProne.Annotations.dll.so => 315
	i64 u0x5906028ae5151104, ; 413: Xamarin.AndroidX.Activity.Ktx => 237
	i64 u0x595a356d23e8da9a, ; 414: lib_Microsoft.CSharp.dll.so => 1
	i64 u0x59a935a032dbc08c, ; 415: lib_Grpc.Auth.dll.so => 195
	i64 u0x59f9e60b9475085f, ; 416: lib_Xamarin.AndroidX.Annotation.Experimental.dll.so => 239
	i64 u0x5a6dc081b000c5d7, ; 417: lib_Xamarin.Grpc.OkHttp.dll.so => 328
	i64 u0x5a745f5101a75527, ; 418: lib_System.IO.Compression.FileSystem.dll.so => 44
	i64 u0x5a89a886ae30258d, ; 419: lib_Xamarin.AndroidX.CoordinatorLayout.dll.so => 253
	i64 u0x5a8f6699f4a1caa9, ; 420: lib_System.Threading.dll.so => 149
	i64 u0x5ae9cd33b15841bf, ; 421: System.ComponentModel => 18
	i64 u0x5aeb8cd498d4823e, ; 422: lib_Xamarin.Google.Guava.dll.so => 317
	i64 u0x5b54391bdc6fcfe6, ; 423: System.Private.DataContractSerialization => 86
	i64 u0x5b5f0e240a06a2a2, ; 424: da/Microsoft.Maui.Controls.resources.dll => 347
	i64 u0x5b755276902c8414, ; 425: Xamarin.GooglePlayServices.Base => 320
	i64 u0x5b8109e8e14c5e3e, ; 426: System.Globalization.Extensions.dll => 41
	i64 u0x5bddd04d72a9e350, ; 427: Xamarin.AndroidX.Lifecycle.LiveData.Core.Ktx => 272
	i64 u0x5bdf16b09da116ab, ; 428: Xamarin.AndroidX.Collection => 247
	i64 u0x5c019d5266093159, ; 429: lib_Xamarin.AndroidX.Lifecycle.Runtime.Ktx.Android.dll.so => 277
	i64 u0x5c30a4a35f9cc8c4, ; 430: lib_System.Reflection.Extensions.dll.so => 94
	i64 u0x5c393624b8176517, ; 431: lib_Microsoft.Extensions.Logging.dll.so => 209
	i64 u0x5c53c29f5073b0c9, ; 432: System.Diagnostics.FileVersionInfo => 28
	i64 u0x5c87463c575c7616, ; 433: lib_System.Globalization.Extensions.dll.so => 41
	i64 u0x5d0a4a29b02d9d3c, ; 434: System.Net.WebHeaderCollection.dll => 78
	i64 u0x5d40c9b15181641f, ; 435: lib_Xamarin.AndroidX.Emoji2.dll.so => 262
	i64 u0x5d6ca10d35e9485b, ; 436: lib_Xamarin.AndroidX.Concurrent.Futures.dll.so => 250
	i64 u0x5d7ec76c1c703055, ; 437: System.Threading.Tasks.Parallel => 144
	i64 u0x5db0cbbd1028510e, ; 438: lib_System.Runtime.InteropServices.dll.so => 108
	i64 u0x5db30905d3e5013b, ; 439: Xamarin.AndroidX.Collection.Jvm.dll => 248
	i64 u0x5e467bc8f09ad026, ; 440: System.Collections.Specialized.dll => 11
	i64 u0x5e5173b3208d97e7, ; 441: System.Runtime.Handles.dll => 105
	i64 u0x5ea92fdb19ec8c4c, ; 442: System.Text.Encodings.Web.dll => 137
	i64 u0x5eb8046dd40e9ac3, ; 443: System.ComponentModel.Primitives => 16
	i64 u0x5ec272d219c9aba4, ; 444: System.Security.Cryptography.Csp.dll => 122
	i64 u0x5eee1376d94c7f5e, ; 445: System.Net.HttpListener.dll => 66
	i64 u0x5f36ccf5c6a57e24, ; 446: System.Xml.ReaderWriter.dll => 157
	i64 u0x5f4294b9b63cb842, ; 447: System.Data.Common => 22
	i64 u0x5f9a2d823f664957, ; 448: lib-el-Microsoft.Maui.Controls.resources.dll.so => 349
	i64 u0x5fa6da9c3cd8142a, ; 449: lib_Xamarin.KotlinX.Serialization.Core.dll.so => 341
	i64 u0x5fac98e0b37a5b9d, ; 450: System.Runtime.CompilerServices.Unsafe.dll => 102
	i64 u0x609f4b7b63d802d4, ; 451: lib_Microsoft.Extensions.DependencyInjection.dll.so => 204
	i64 u0x60cd4e33d7e60134, ; 452: Xamarin.KotlinX.Coroutines.Core.Jvm => 340
	i64 u0x60f62d786afcf130, ; 453: System.Memory => 63
	i64 u0x61bb78c89f867353, ; 454: System.IO => 58
	i64 u0x61be8d1299194243, ; 455: Microsoft.Maui.Controls.Xaml => 217
	i64 u0x61d2cba29557038f, ; 456: de/Microsoft.Maui.Controls.resources => 348
	i64 u0x61d88f399afb2f45, ; 457: lib_System.Runtime.Loader.dll.so => 110
	i64 u0x622eef6f9e59068d, ; 458: System.Private.CoreLib => 173
	i64 u0x62e976fd765a2339, ; 459: Xamarin.Firebase.Auth.Interop => 305
	i64 u0x63982c87366f9be8, ; 460: Xamarin.Google.Guava => 317
	i64 u0x63cdbd66ac39bb46, ; 461: lib_Microsoft.VisualStudio.DesignTools.XamlTapContract.dll.so => 381
	i64 u0x63d5e3aa4ef9b931, ; 462: Xamarin.KotlinX.Coroutines.Android.dll => 338
	i64 u0x63f1f6883c1e23c2, ; 463: lib_System.Collections.Immutable.dll.so => 9
	i64 u0x6400f68068c1e9f1, ; 464: Xamarin.Google.Android.Material.dll => 311
	i64 u0x640e3b14dbd325c2, ; 465: System.Security.Cryptography.Algorithms.dll => 120
	i64 u0x64587004560099b9, ; 466: System.Reflection => 98
	i64 u0x64b1529a438a3c45, ; 467: lib_System.Runtime.Handles.dll.so => 105
	i64 u0x6533c154f14eefe0, ; 468: lib_Google.Api.Gax.Rest.dll.so => 182
	i64 u0x6565fba2cd8f235b, ; 469: Xamarin.AndroidX.Lifecycle.ViewModel.Ktx => 280
	i64 u0x65ecac39144dd3cc, ; 470: Microsoft.Maui.Controls.dll => 215
	i64 u0x65ece51227bfa724, ; 471: lib_System.Runtime.Numerics.dll.so => 111
	i64 u0x661722438787b57f, ; 472: Xamarin.AndroidX.Annotation.Jvm.dll => 240
	i64 u0x6679b2337ee6b22a, ; 473: lib_System.IO.FileSystem.Primitives.dll.so => 49
	i64 u0x6692e924eade1b29, ; 474: lib_System.Console.dll.so => 20
	i64 u0x66a4e5c6a3fb0bae, ; 475: lib_Xamarin.AndroidX.Lifecycle.ViewModel.Android.dll.so => 279
	i64 u0x66d13304ce1a3efa, ; 476: Xamarin.AndroidX.CursorAdapter => 257
	i64 u0x674303f65d8fad6f, ; 477: lib_System.Net.Quic.dll.so => 72
	i64 u0x6756ca4cad62e9d6, ; 478: lib_Xamarin.AndroidX.ConstraintLayout.Core.dll.so => 252
	i64 u0x67c0802770244408, ; 479: System.Windows.dll => 155
	i64 u0x68100b69286e27cd, ; 480: lib_System.Formats.Tar.dll.so => 39
	i64 u0x68558ec653afa616, ; 481: lib-da-Microsoft.Maui.Controls.resources.dll.so => 347
	i64 u0x6872ec7a2e36b1ac, ; 482: System.Drawing.Primitives.dll => 35
	i64 u0x68bb2c417aa9b61c, ; 483: Xamarin.KotlinX.AtomicFU.dll => 336
	i64 u0x68fbbbe2eb455198, ; 484: System.Formats.Asn1 => 38
	i64 u0x69063fc0ba8e6bdd, ; 485: he/Microsoft.Maui.Controls.resources.dll => 353
	i64 u0x6938a76e7814446f, ; 486: Maui.GoogleMaps.dll => 223
	i64 u0x69a3e26c76f6eec4, ; 487: Xamarin.AndroidX.Window.Extensions.Core.Core.dll => 304
	i64 u0x6a4d7577b2317255, ; 488: System.Runtime.InteropServices.dll => 108
	i64 u0x6ace3b74b15ee4a4, ; 489: nb/Microsoft.Maui.Controls.resources => 362
	i64 u0x6afcedb171067e2b, ; 490: System.Core.dll => 21
	i64 u0x6b2b13561049ea2c, ; 491: lib_Xamarin.Protobuf.Lite.dll.so => 343
	i64 u0x6bc822f45373a1d6, ; 492: Google.Apis.dll => 183
	i64 u0x6bef98e124147c24, ; 493: Xamarin.Jetbrains.Annotations => 333
	i64 u0x6ce874bff138ce2b, ; 494: Xamarin.AndroidX.Lifecycle.ViewModel.dll => 278
	i64 u0x6d12bfaa99c72b1f, ; 495: lib_Microsoft.Maui.Graphics.dll.so => 220
	i64 u0x6d70755158ca866e, ; 496: lib_System.ComponentModel.EventBasedAsync.dll.so => 15
	i64 u0x6d79993361e10ef2, ; 497: Microsoft.Extensions.Primitives => 214
	i64 u0x6d7eeca99577fc8b, ; 498: lib_System.Net.WebProxy.dll.so => 79
	i64 u0x6d8515b19946b6a2, ; 499: System.Net.WebProxy.dll => 79
	i64 u0x6d86d56b84c8eb71, ; 500: lib_Xamarin.AndroidX.CursorAdapter.dll.so => 257
	i64 u0x6d9bea6b3e895cf7, ; 501: Microsoft.Extensions.Primitives.dll => 214
	i64 u0x6e25a02c3833319a, ; 502: lib_Xamarin.AndroidX.Navigation.Fragment.dll.so => 284
	i64 u0x6e79c6bd8627412a, ; 503: Xamarin.AndroidX.SavedState.SavedState.Ktx => 291
	i64 u0x6e838d9a2a6f6c9e, ; 504: lib_System.ValueTuple.dll.so => 152
	i64 u0x6e9965ce1095e60a, ; 505: lib_System.Core.dll.so => 21
	i64 u0x6fd2265da78b93a4, ; 506: lib_Microsoft.Maui.dll.so => 218
	i64 u0x6fdfc7de82c33008, ; 507: cs/Microsoft.Maui.Controls.resources => 346
	i64 u0x6ffc4967cc47ba57, ; 508: System.IO.FileSystem.Watcher.dll => 50
	i64 u0x701cd46a1c25a5fe, ; 509: System.IO.FileSystem.dll => 51
	i64 u0x70664ad3307f4fbf, ; 510: Xamarin.Grpc.Core.dll => 327
	i64 u0x70e99f48c05cb921, ; 511: tr/Microsoft.Maui.Controls.resources.dll => 372
	i64 u0x70fb7a3521043a40, ; 512: Plugin.CloudFirestore => 225
	i64 u0x70fd3deda22442d2, ; 513: lib-nb-Microsoft.Maui.Controls.resources.dll.so => 362
	i64 u0x71485e7ffdb4b958, ; 514: System.Reflection.Extensions => 94
	i64 u0x7162a2fce67a945f, ; 515: lib_Xamarin.Android.Glide.Annotations.dll.so => 233
	i64 u0x717530326f808838, ; 516: lib_Microsoft.Extensions.Diagnostics.Abstractions.dll.so => 207
	i64 u0x71a495ea3761dde8, ; 517: lib-it-Microsoft.Maui.Controls.resources.dll.so => 358
	i64 u0x71ad672adbe48f35, ; 518: System.ComponentModel.Primitives.dll => 16
	i64 u0x720f102581a4a5c8, ; 519: Xamarin.AndroidX.Core.ViewTree => 256
	i64 u0x725f5a9e82a45c81, ; 520: System.Security.Cryptography.Encoding => 123
	i64 u0x72b1fb4109e08d7b, ; 521: lib-hr-Microsoft.Maui.Controls.resources.dll.so => 355
	i64 u0x72e0300099accce1, ; 522: System.Xml.XPath.XDocument => 160
	i64 u0x730bfb248998f67a, ; 523: System.IO.Compression.ZipFile => 45
	i64 u0x732b2d67b9e5c47b, ; 524: Xamarin.Google.ErrorProne.Annotations.dll => 315
	i64 u0x734b76fdc0dc05bb, ; 525: lib_GoogleGson.dll.so => 194
	i64 u0x73a6be34e822f9d1, ; 526: lib_System.Runtime.Serialization.dll.so => 116
	i64 u0x73e4ce94e2eb6ffc, ; 527: lib_System.Memory.dll.so => 63
	i64 u0x743a1eccf080489a, ; 528: WindowsBase.dll => 166
	i64 u0x746cf89b511b4d40, ; 529: lib_Microsoft.Extensions.Diagnostics.dll.so => 206
	i64 u0x755a91767330b3d4, ; 530: lib_Microsoft.Extensions.Configuration.dll.so => 201
	i64 u0x75c326eb821b85c4, ; 531: lib_System.ComponentModel.DataAnnotations.dll.so => 14
	i64 u0x76012e7334db86e5, ; 532: lib_Xamarin.AndroidX.SavedState.dll.so => 290
	i64 u0x76ca07b878f44da0, ; 533: System.Runtime.Numerics.dll => 111
	i64 u0x7736c8a96e51a061, ; 534: lib_Xamarin.AndroidX.Annotation.Jvm.dll.so => 240
	i64 u0x776219ac62545ae1, ; 535: Acr.UserDialogs.Maui.dll => 175
	i64 u0x778a805e625329ef, ; 536: System.Linq.Parallel => 60
	i64 u0x779290cc2b801eb7, ; 537: Xamarin.KotlinX.AtomicFU.Jvm => 337
	i64 u0x77bf40592cd67602, ; 538: Xamarin.Google.AutoValue.Annotations.dll => 312
	i64 u0x77d48bf846bc0f10, ; 539: Xamarin.Io.OpenCensus.OpenCensusContribGrpcMetrics => 332
	i64 u0x77f8a4acc2fdc449, ; 540: System.Security.Cryptography.Cng.dll => 121
	i64 u0x780bc73597a503a9, ; 541: lib-ms-Microsoft.Maui.Controls.resources.dll.so => 361
	i64 u0x782c5d8eb99ff201, ; 542: lib_Microsoft.VisualBasic.Core.dll.so => 2
	i64 u0x783606d1e53e7a1a, ; 543: th/Microsoft.Maui.Controls.resources.dll => 371
	i64 u0x784b4ff3eed363ff, ; 544: Xamarin.Firebase.Common => 306
	i64 u0x78a45e51311409b6, ; 545: Xamarin.AndroidX.Fragment.dll => 265
	i64 u0x78ed4ab8f9d800a1, ; 546: Xamarin.AndroidX.Lifecycle.ViewModel => 278
	i64 u0x7939c1796e1e9b03, ; 547: Square.OkHttp.dll => 226
	i64 u0x7a25bdb29108c6e7, ; 548: Microsoft.Extensions.Http => 208
	i64 u0x7a39601d6f0bb831, ; 549: lib_Xamarin.KotlinX.AtomicFU.dll.so => 336
	i64 u0x7a5207a7c82d30b4, ; 550: lib_Xamarin.JSpecify.dll.so => 334
	i64 u0x7a7e7eddf79c5d26, ; 551: lib_Xamarin.AndroidX.Lifecycle.ViewModel.dll.so => 278
	i64 u0x7a9a57d43b0845fa, ; 552: System.AppContext => 6
	i64 u0x7ad0f4f1e5d08183, ; 553: Xamarin.AndroidX.Collection.dll => 247
	i64 u0x7adb8da2ac89b647, ; 554: fi/Microsoft.Maui.Controls.resources.dll => 351
	i64 u0x7b13d9eaa944ade8, ; 555: Xamarin.AndroidX.DynamicAnimation.dll => 261
	i64 u0x7b38c8e2aa084ac3, ; 556: Google.Apis.Bigquery.v2.dll => 185
	i64 u0x7bef86a4335c4870, ; 557: System.ComponentModel.TypeConverter => 17
	i64 u0x7c0820144cd34d6a, ; 558: sk/Microsoft.Maui.Controls.resources.dll => 369
	i64 u0x7c2a0bd1e0f988fc, ; 559: lib-de-Microsoft.Maui.Controls.resources.dll.so => 348
	i64 u0x7c41d387501568ba, ; 560: System.Net.WebClient.dll => 77
	i64 u0x7c482cd79bd24b13, ; 561: lib_Xamarin.AndroidX.ConstraintLayout.dll.so => 251
	i64 u0x7c8cb8cf04bee12b, ; 562: lib_Xamarin.Google.AutoValue.Annotations.dll.so => 312
	i64 u0x7cb95ad2a929d044, ; 563: Xamarin.GooglePlayServices.Basement => 321
	i64 u0x7cd2ec8eaf5241cd, ; 564: System.Security.dll => 131
	i64 u0x7cf9ae50dd350622, ; 565: Xamarin.Jetbrains.Annotations.dll => 333
	i64 u0x7d649b75d580bb42, ; 566: ms/Microsoft.Maui.Controls.resources.dll => 361
	i64 u0x7d8ee2bdc8e3aad1, ; 567: System.Numerics.Vectors => 83
	i64 u0x7df5df8db8eaa6ac, ; 568: Microsoft.Extensions.Logging.Debug => 211
	i64 u0x7dfc3d6d9d8d7b70, ; 569: System.Collections => 12
	i64 u0x7e2e564fa2f76c65, ; 570: lib_System.Diagnostics.Tracing.dll.so => 34
	i64 u0x7e302e110e1e1346, ; 571: lib_System.Security.Claims.dll.so => 119
	i64 u0x7e4465b3f78ad8d0, ; 572: Xamarin.KotlinX.Serialization.Core.dll => 341
	i64 u0x7e571cad5915e6c3, ; 573: lib_Xamarin.AndroidX.Lifecycle.Process.dll.so => 273
	i64 u0x7e65340ed0da76d2, ; 574: Xamarin.Grpc.OkHttp => 328
	i64 u0x7e6b1ca712437d7d, ; 575: Xamarin.AndroidX.Emoji2.ViewsHelper => 263
	i64 u0x7e946809d6008ef2, ; 576: lib_System.ObjectModel.dll.so => 85
	i64 u0x7ea0272c1b4a9635, ; 577: lib_Xamarin.Android.Glide.dll.so => 232
	i64 u0x7eb4f0dc47488736, ; 578: lib_Xamarin.GooglePlayServices.Tasks.dll.so => 323
	i64 u0x7ecc13347c8fd849, ; 579: lib_System.ComponentModel.dll.so => 18
	i64 u0x7f00ddd9b9ca5a13, ; 580: Xamarin.AndroidX.ViewPager.dll => 301
	i64 u0x7f9351cd44b1273f, ; 581: Microsoft.Extensions.Configuration.Abstractions => 202
	i64 u0x7fbd557c99b3ce6f, ; 582: lib_Xamarin.AndroidX.Lifecycle.LiveData.Core.dll.so => 271
	i64 u0x8076a9a44a2ca331, ; 583: System.Net.Quic => 72
	i64 u0x80b7e726b0280681, ; 584: Microsoft.VisualStudio.DesignTools.MobileTapContracts => 379
	i64 u0x80da183a87731838, ; 585: System.Reflection.Metadata => 95
	i64 u0x812c069d5cdecc17, ; 586: System.dll => 165
	i64 u0x81381be520a60adb, ; 587: Xamarin.AndroidX.Interpolator.dll => 267
	i64 u0x8145faf772692484, ; 588: Google.Cloud.Firestore.V1.dll => 189
	i64 u0x81657cec2b31e8aa, ; 589: System.Net => 82
	i64 u0x81ab745f6c0f5ce6, ; 590: zh-Hant/Microsoft.Maui.Controls.resources => 377
	i64 u0x8277f2be6b5ce05f, ; 591: Xamarin.AndroidX.AppCompat => 241
	i64 u0x828f06563b30bc50, ; 592: lib_Xamarin.AndroidX.CardView.dll.so => 246
	i64 u0x82920a8d9194a019, ; 593: Xamarin.KotlinX.AtomicFU.Jvm.dll => 337
	i64 u0x82b399cb01b531c4, ; 594: lib_System.Web.dll.so => 154
	i64 u0x82df8f5532a10c59, ; 595: lib_System.Drawing.dll.so => 36
	i64 u0x82f0b6e911d13535, ; 596: lib_System.Transactions.dll.so => 151
	i64 u0x82f6403342e12049, ; 597: uk/Microsoft.Maui.Controls.resources => 373
	i64 u0x83c14ba66c8e2b8c, ; 598: zh-Hans/Microsoft.Maui.Controls.resources => 376
	i64 u0x846ce984efea52c7, ; 599: System.Threading.Tasks.Parallel.dll => 144
	i64 u0x84ae73148a4557d2, ; 600: lib_System.IO.Pipes.dll.so => 56
	i64 u0x84b01102c12a9232, ; 601: System.Runtime.Serialization.Json.dll => 113
	i64 u0x850c5ba0b57ce8e7, ; 602: lib_Xamarin.AndroidX.Collection.dll.so => 247
	i64 u0x851d02edd334b044, ; 603: Xamarin.AndroidX.VectorDrawable => 298
	i64 u0x85410a0ce2b82e74, ; 604: lib_Xamarin.Grpc.Context.dll.so => 326
	i64 u0x85c919db62150978, ; 605: Xamarin.AndroidX.Transition.dll => 297
	i64 u0x8662aaeb94fef37f, ; 606: lib_System.Dynamic.Runtime.dll.so => 37
	i64 u0x866c029b39d9cde6, ; 607: Plugin.CloudFirestore.dll => 225
	i64 u0x86a909228dc7657b, ; 608: lib-zh-Hant-Microsoft.Maui.Controls.resources.dll.so => 377
	i64 u0x86b3e00c36b84509, ; 609: Microsoft.Extensions.Configuration.dll => 201
	i64 u0x86b62cb077ec4fd7, ; 610: System.Runtime.Serialization.Xml => 115
	i64 u0x8706ffb12bf3f53d, ; 611: Xamarin.AndroidX.Annotation.Experimental => 239
	i64 u0x872a5b14c18d328c, ; 612: System.ComponentModel.DataAnnotations => 14
	i64 u0x872fb9615bc2dff0, ; 613: Xamarin.Android.Glide.Annotations.dll => 233
	i64 u0x8794c7c19600413d, ; 614: Xamarin.Grpc.Protobuf.Lite => 329
	i64 u0x87b7bede2c8fef74, ; 615: Xamarin.Firebase.ProtoliteWellKnownTypes.dll => 310
	i64 u0x87c69b87d9283884, ; 616: lib_System.Threading.Thread.dll.so => 146
	i64 u0x87f6569b25707834, ; 617: System.IO.Compression.Brotli.dll => 43
	i64 u0x87fef727071b7fe5, ; 618: Grpc.Net.Client => 197
	i64 u0x8842b3a5d2d3fb36, ; 619: Microsoft.Maui.Essentials => 219
	i64 u0x88926583efe7ee86, ; 620: Xamarin.AndroidX.Activity.Ktx.dll => 237
	i64 u0x88ba6bc4f7762b03, ; 621: lib_System.Reflection.dll.so => 98
	i64 u0x88bda98e0cffb7a9, ; 622: lib_Xamarin.KotlinX.Coroutines.Core.Jvm.dll.so => 340
	i64 u0x8930322c7bd8f768, ; 623: netstandard => 168
	i64 u0x897a606c9e39c75f, ; 624: lib_System.ComponentModel.Primitives.dll.so => 16
	i64 u0x89911a22005b92b7, ; 625: System.IO.FileSystem.DriveInfo.dll => 48
	i64 u0x89c5188089ec2cd5, ; 626: lib_System.Runtime.InteropServices.RuntimeInformation.dll.so => 107
	i64 u0x8a19e3dc71b34b2c, ; 627: System.Reflection.TypeExtensions.dll => 97
	i64 u0x8a90bab2026e5b88, ; 628: Google.Cloud.Firestore.dll => 188
	i64 u0x8ad229ea26432ee2, ; 629: Xamarin.AndroidX.Loader => 282
	i64 u0x8b4ff5d0fdd5faa1, ; 630: lib_System.Diagnostics.DiagnosticSource.dll.so => 27
	i64 u0x8b541d476eb3774c, ; 631: System.Security.Principal.Windows => 128
	i64 u0x8b8d01333a96d0b5, ; 632: System.Diagnostics.Process.dll => 29
	i64 u0x8b9ceca7acae3451, ; 633: lib-he-Microsoft.Maui.Controls.resources.dll.so => 353
	i64 u0x8c230514448fef34, ; 634: Xamarin.Firebase.ProtoliteWellKnownTypes => 310
	i64 u0x8c7c5e0e7a582f61, ; 635: lib_Acr.UserDialogs.Maui.dll.so => 175
	i64 u0x8cb8f612b633affb, ; 636: Xamarin.AndroidX.SavedState.SavedState.Ktx.dll => 291
	i64 u0x8ccdb7a916d5b2b6, ; 637: ISO3166.dll => 199
	i64 u0x8cdfdb4ce85fb925, ; 638: lib_System.Security.Principal.Windows.dll.so => 128
	i64 u0x8cdfe7b8f4caa426, ; 639: System.IO.Compression.FileSystem => 44
	i64 u0x8d0f420977c2c1c7, ; 640: Xamarin.AndroidX.CursorAdapter.dll => 257
	i64 u0x8d52f7ea2796c531, ; 641: Xamarin.AndroidX.Emoji2.dll => 262
	i64 u0x8d7b8ab4b3310ead, ; 642: System.Threading => 149
	i64 u0x8da188285aadfe8e, ; 643: System.Collections.Concurrent => 8
	i64 u0x8dfc1cfbf8858f95, ; 644: Grpc.Core.Api.dll => 196
	i64 u0x8e68459d22cb214f, ; 645: Square.OkHttp => 226
	i64 u0x8ec6e06a61c1baeb, ; 646: lib_Newtonsoft.Json.dll.so => 222
	i64 u0x8ed807bfe9858dfc, ; 647: Xamarin.AndroidX.Navigation.Common => 283
	i64 u0x8ee08b8194a30f48, ; 648: lib-hi-Microsoft.Maui.Controls.resources.dll.so => 354
	i64 u0x8ef7601039857a44, ; 649: lib-ro-Microsoft.Maui.Controls.resources.dll.so => 367
	i64 u0x8efbc0801a122264, ; 650: Xamarin.GooglePlayServices.Tasks.dll => 323
	i64 u0x8f32c6f611f6ffab, ; 651: pt/Microsoft.Maui.Controls.resources.dll => 366
	i64 u0x8f44b45eb046bbd1, ; 652: System.ServiceModel.Web.dll => 132
	i64 u0x8f8829d21c8985a4, ; 653: lib-pt-BR-Microsoft.Maui.Controls.resources.dll.so => 365
	i64 u0x8fbf5b0114c6dcef, ; 654: System.Globalization.dll => 42
	i64 u0x8fcc8c2a81f3d9e7, ; 655: Xamarin.KotlinX.Serialization.Core => 341
	i64 u0x8fd42635e63de49e, ; 656: Xamarin.Grpc.Context.dll => 326
	i64 u0x90263f8448b8f572, ; 657: lib_System.Diagnostics.TraceSource.dll.so => 33
	i64 u0x903101b46fb73a04, ; 658: _Microsoft.Android.Resource.Designer => 382
	i64 u0x90393bd4865292f3, ; 659: lib_System.IO.Compression.dll.so => 46
	i64 u0x905e2b8e7ae91ae6, ; 660: System.Threading.Tasks.Extensions.dll => 143
	i64 u0x90634f86c5ebe2b5, ; 661: Xamarin.AndroidX.Lifecycle.ViewModel.Android => 279
	i64 u0x907b636704ad79ef, ; 662: lib_Microsoft.Maui.Controls.Xaml.dll.so => 217
	i64 u0x90e9efbfd68593e0, ; 663: lib_Xamarin.AndroidX.Lifecycle.LiveData.dll.so => 270
	i64 u0x91418dc638b29e68, ; 664: lib_Xamarin.AndroidX.CustomView.dll.so => 258
	i64 u0x9157bd523cd7ed36, ; 665: lib_System.Text.Json.dll.so => 138
	i64 u0x91a74f07b30d37e2, ; 666: System.Linq.dll => 62
	i64 u0x91cb86ea3b17111d, ; 667: System.ServiceModel.Web => 132
	i64 u0x91fa41a87223399f, ; 668: ca/Microsoft.Maui.Controls.resources.dll => 345
	i64 u0x92054e486c0c7ea7, ; 669: System.IO.FileSystem.DriveInfo => 48
	i64 u0x928614058c40c4cd, ; 670: lib_System.Xml.XPath.XDocument.dll.so => 160
	i64 u0x92a698e6d582778f, ; 671: Xamarin.Firebase.Components.dll => 307
	i64 u0x92b138fffca2b01e, ; 672: lib_Xamarin.AndroidX.Arch.Core.Runtime.dll.so => 244
	i64 u0x92dfc2bfc6c6a888, ; 673: Xamarin.AndroidX.Lifecycle.LiveData => 270
	i64 u0x933da2c779423d68, ; 674: Xamarin.Android.Glide.Annotations => 233
	i64 u0x9388aad9b7ae40ce, ; 675: lib_Xamarin.AndroidX.Lifecycle.Common.dll.so => 268
	i64 u0x93cfa73ab28d6e35, ; 676: ms/Microsoft.Maui.Controls.resources => 361
	i64 u0x941c00d21e5c0679, ; 677: lib_Xamarin.AndroidX.Transition.dll.so => 297
	i64 u0x944077d8ca3c6580, ; 678: System.IO.Compression.dll => 46
	i64 u0x948cffedc8ed7960, ; 679: System.Xml => 164
	i64 u0x94c8990839c4bdb1, ; 680: lib_Xamarin.AndroidX.Interpolator.dll.so => 267
	i64 u0x967fc325e09bfa8c, ; 681: es/Microsoft.Maui.Controls.resources => 350
	i64 u0x9686161486d34b81, ; 682: lib_Xamarin.AndroidX.ExifInterface.dll.so => 264
	i64 u0x9729c8c4c069c478, ; 683: Google.Apis.Core => 186
	i64 u0x9732d8dbddea3d9a, ; 684: id/Microsoft.Maui.Controls.resources => 357
	i64 u0x978be80e5210d31b, ; 685: Microsoft.Maui.Graphics.dll => 220
	i64 u0x979ab54025cc1c7f, ; 686: lib_Xamarin.GooglePlayServices.Base.dll.so => 320
	i64 u0x97b8c771ea3e4220, ; 687: System.ComponentModel.dll => 18
	i64 u0x97e144c9d3c6976e, ; 688: System.Collections.Concurrent.dll => 8
	i64 u0x97ecf6eef5bb6802, ; 689: CFAN.SchoolMap.Maui.dll => 0
	i64 u0x984184e3c70d4419, ; 690: GoogleGson => 194
	i64 u0x9843944103683dd3, ; 691: Xamarin.AndroidX.Core.Core.Ktx => 255
	i64 u0x98d720cc4597562c, ; 692: System.Security.Cryptography.OpenSsl => 124
	i64 u0x991d510397f92d9d, ; 693: System.Linq.Expressions => 59
	i64 u0x996ceeb8a3da3d67, ; 694: System.Threading.Overlapped.dll => 141
	i64 u0x999cb19e1a04ffd3, ; 695: CommunityToolkit.Mvvm.dll => 177
	i64 u0x99a00ca5270c6878, ; 696: Xamarin.AndroidX.Navigation.Runtime => 285
	i64 u0x99cdc6d1f2d3a72f, ; 697: ko/Microsoft.Maui.Controls.resources.dll => 360
	i64 u0x9a01b1da98b6ee10, ; 698: Xamarin.AndroidX.Lifecycle.Runtime.dll => 274
	i64 u0x9a2d4c8408e9f4b6, ; 699: lib_Plugin.CloudFirestore.dll.so => 225
	i64 u0x9a5ccc274fd6e6ee, ; 700: Jsr305Binding.dll => 313
	i64 u0x9ae6940b11c02876, ; 701: lib_Xamarin.AndroidX.Window.dll.so => 303
	i64 u0x9b211a749105beac, ; 702: System.Transactions.Local => 150
	i64 u0x9b8734714671022d, ; 703: System.Threading.Tasks.Dataflow.dll => 142
	i64 u0x9bc6aea27fbf034f, ; 704: lib_Xamarin.KotlinX.Coroutines.Core.dll.so => 339
	i64 u0x9bd8cc74558ad4c7, ; 705: Xamarin.KotlinX.AtomicFU => 336
	i64 u0x9c244ac7cda32d26, ; 706: System.Security.Cryptography.X509Certificates.dll => 126
	i64 u0x9c465f280cf43733, ; 707: lib_Xamarin.KotlinX.Coroutines.Android.dll.so => 338
	i64 u0x9c8f6872beab6408, ; 708: System.Xml.XPath.XDocument.dll => 160
	i64 u0x9ce01cf91101ae23, ; 709: System.Xml.XmlDocument => 162
	i64 u0x9d128180c81d7ce6, ; 710: Xamarin.AndroidX.CustomView.PoolingContainer => 259
	i64 u0x9d5dbcf5a48583fe, ; 711: lib_Xamarin.AndroidX.Activity.dll.so => 236
	i64 u0x9d74dee1a7725f34, ; 712: Microsoft.Extensions.Configuration.Abstractions.dll => 202
	i64 u0x9e4534b6adaf6e84, ; 713: nl/Microsoft.Maui.Controls.resources => 363
	i64 u0x9e4b95dec42769f7, ; 714: System.Diagnostics.Debug.dll => 26
	i64 u0x9eaf1efdf6f7267e, ; 715: Xamarin.AndroidX.Navigation.Common.dll => 283
	i64 u0x9ef542cf1f78c506, ; 716: Xamarin.AndroidX.Lifecycle.LiveData.Core => 271
	i64 u0x9f2c1126c41c6e52, ; 717: lib_Xamarin.Io.OpenCensus.OpenCensusContribGrpcMetrics.dll.so => 332
	i64 u0xa00832eb975f56a8, ; 718: lib_System.Net.dll.so => 82
	i64 u0xa0ad78236b7b267f, ; 719: Xamarin.AndroidX.Window => 303
	i64 u0xa0d8259f4cc284ec, ; 720: lib_System.Security.Cryptography.dll.so => 127
	i64 u0xa0e17ca50c77a225, ; 721: lib_Xamarin.Google.Crypto.Tink.Android.dll.so => 314
	i64 u0xa0ff9b3e34d92f11, ; 722: lib_System.Resources.Writer.dll.so => 101
	i64 u0xa12fbfb4da97d9f3, ; 723: System.Threading.Timer.dll => 148
	i64 u0xa1440773ee9d341e, ; 724: Xamarin.Google.Android.Material => 311
	i64 u0xa1a269041503c365, ; 725: AndHUD => 176
	i64 u0xa1b9d7c27f47219f, ; 726: Xamarin.AndroidX.Navigation.UI.dll => 286
	i64 u0xa2572680829d2c7c, ; 727: System.IO.Pipelines.dll => 54
	i64 u0xa26597e57ee9c7f6, ; 728: System.Xml.XmlDocument.dll => 162
	i64 u0xa308401900e5bed3, ; 729: lib_mscorlib.dll.so => 167
	i64 u0xa395572e7da6c99d, ; 730: lib_System.Security.dll.so => 131
	i64 u0xa3e683f24b43af6f, ; 731: System.Dynamic.Runtime.dll => 37
	i64 u0xa4145becdee3dc4f, ; 732: Xamarin.AndroidX.VectorDrawable.Animated => 299
	i64 u0xa46aa1eaa214539b, ; 733: ko/Microsoft.Maui.Controls.resources => 360
	i64 u0xa4a372eecb9e4df0, ; 734: Microsoft.Extensions.Diagnostics => 206
	i64 u0xa4d20d2ff0563d26, ; 735: lib_CommunityToolkit.Mvvm.dll.so => 177
	i64 u0xa4edc8f2ceae241a, ; 736: System.Data.Common.dll => 22
	i64 u0xa5494f40f128ce6a, ; 737: System.Runtime.Serialization.Formatters.dll => 112
	i64 u0xa54b74df83dce92b, ; 738: System.Reflection.DispatchProxy => 90
	i64 u0xa5b7152421ed6d98, ; 739: lib_System.IO.FileSystem.Watcher.dll.so => 50
	i64 u0xa5c3844f17b822db, ; 740: lib_System.Linq.Parallel.dll.so => 60
	i64 u0xa5ce5c755bde8cb8, ; 741: lib_System.Security.Cryptography.Csp.dll.so => 122
	i64 u0xa5e599d1e0524750, ; 742: System.Numerics.Vectors.dll => 83
	i64 u0xa5f1ba49b85dd355, ; 743: System.Security.Cryptography.dll => 127
	i64 u0xa5f1e826b58a6998, ; 744: System.Linq.Async.dll => 229
	i64 u0xa61975a5a37873ea, ; 745: lib_System.Xml.XmlSerializer.dll.so => 163
	i64 u0xa6593e21584384d2, ; 746: lib_Jsr305Binding.dll.so => 313
	i64 u0xa66cbee0130865f7, ; 747: lib_WindowsBase.dll.so => 166
	i64 u0xa67dbee13e1df9ca, ; 748: Xamarin.AndroidX.SavedState.dll => 290
	i64 u0xa684b098dd27b296, ; 749: lib_Xamarin.AndroidX.Security.SecurityCrypto.dll.so => 292
	i64 u0xa68a420042bb9b1f, ; 750: Xamarin.AndroidX.DrawerLayout.dll => 260
	i64 u0xa6d26156d1cacc7c, ; 751: Xamarin.Android.Glide.dll => 232
	i64 u0xa75386b5cb9595aa, ; 752: Xamarin.AndroidX.Lifecycle.Runtime.Android => 275
	i64 u0xa763fbb98df8d9fb, ; 753: lib_Microsoft.Win32.Primitives.dll.so => 4
	i64 u0xa78ce3745383236a, ; 754: Xamarin.AndroidX.Lifecycle.Common.Jvm => 269
	i64 u0xa7c31b56b4dc7b33, ; 755: hu/Microsoft.Maui.Controls.resources => 356
	i64 u0xa7eab29ed44b4e7a, ; 756: Mono.Android.Export => 170
	i64 u0xa8195217cbf017b7, ; 757: Microsoft.VisualBasic.Core => 2
	i64 u0xa843f6095f0d247d, ; 758: Xamarin.GooglePlayServices.Base.dll => 320
	i64 u0xa859a95830f367ff, ; 759: lib_Xamarin.AndroidX.Lifecycle.ViewModel.Ktx.dll.so => 280
	i64 u0xa8b52f21e0dbe690, ; 760: System.Runtime.Serialization.dll => 116
	i64 u0xa8c84ce526c2b4bd, ; 761: Microsoft.VisualStudio.DesignTools.XamlTapContract.dll => 381
	i64 u0xa8ee4ed7de2efaee, ; 762: Xamarin.AndroidX.Annotation.dll => 238
	i64 u0xa952cc4a0d808a59, ; 763: lib_Google.Api.CommonProtos.dll.so => 179
	i64 u0xa95590e7c57438a4, ; 764: System.Configuration => 19
	i64 u0xaa2219c8e3449ff5, ; 765: Microsoft.Extensions.Logging.Abstractions => 210
	i64 u0xaa443ac34067eeef, ; 766: System.Private.Xml.dll => 89
	i64 u0xaa52de307ef5d1dd, ; 767: System.Net.Http => 65
	i64 u0xaa9a7b0214a5cc5c, ; 768: System.Diagnostics.StackTrace.dll => 30
	i64 u0xaaaf86367285a918, ; 769: Microsoft.Extensions.DependencyInjection.Abstractions.dll => 205
	i64 u0xaaf84bb3f052a265, ; 770: el/Microsoft.Maui.Controls.resources => 349
	i64 u0xab375658f5084c9f, ; 771: lib_Google.Cloud.Firestore.dll.so => 188
	i64 u0xab9af77b5b67a0b8, ; 772: Xamarin.AndroidX.ConstraintLayout.Core => 252
	i64 u0xab9c1b2687d86b0b, ; 773: lib_System.Linq.Expressions.dll.so => 59
	i64 u0xac2af3fa195a15ce, ; 774: System.Runtime.Numerics => 111
	i64 u0xac5376a2a538dc10, ; 775: Xamarin.AndroidX.Lifecycle.LiveData.Core.dll => 271
	i64 u0xac5acae88f60357e, ; 776: System.Diagnostics.Tools.dll => 32
	i64 u0xac65e40f62b6b90e, ; 777: Google.Protobuf => 192
	i64 u0xac79c7e46047ad98, ; 778: System.Security.Principal.Windows.dll => 128
	i64 u0xac98d31068e24591, ; 779: System.Xml.XDocument => 159
	i64 u0xacd46e002c3ccb97, ; 780: ro/Microsoft.Maui.Controls.resources => 367
	i64 u0xacdd9e4180d56dda, ; 781: Xamarin.AndroidX.Concurrent.Futures => 250
	i64 u0xacf42eea7ef9cd12, ; 782: System.Threading.Channels => 140
	i64 u0xad89c07347f1bad6, ; 783: nl/Microsoft.Maui.Controls.resources.dll => 363
	i64 u0xadbb53caf78a79d2, ; 784: System.Web.HttpUtility => 153
	i64 u0xadc90ab061a9e6e4, ; 785: System.ComponentModel.TypeConverter.dll => 17
	i64 u0xadca1b9030b9317e, ; 786: Xamarin.AndroidX.Collection.Ktx => 249
	i64 u0xadd8eda2edf396ad, ; 787: Xamarin.Android.Glide.GifDecoder => 235
	i64 u0xadf4cf30debbeb9a, ; 788: System.Net.ServicePoint.dll => 75
	i64 u0xadf511667bef3595, ; 789: System.Net.Security => 74
	i64 u0xae0aaa94fdcfce0f, ; 790: System.ComponentModel.EventBasedAsync.dll => 15
	i64 u0xae282bcd03739de7, ; 791: Java.Interop => 169
	i64 u0xae53579c90db1107, ; 792: System.ObjectModel.dll => 85
	i64 u0xaec7c0c7e2ed4575, ; 793: lib_Xamarin.KotlinX.AtomicFU.Jvm.dll.so => 337
	i64 u0xaf732d0b2193b8f5, ; 794: System.Security.Cryptography.OpenSsl.dll => 124
	i64 u0xafdb94dbccd9d11c, ; 795: Xamarin.AndroidX.Lifecycle.LiveData.dll => 270
	i64 u0xafe29f45095518e7, ; 796: lib_Xamarin.AndroidX.Lifecycle.ViewModelSavedState.dll.so => 281
	i64 u0xb03ae931fb25607e, ; 797: Xamarin.AndroidX.ConstraintLayout => 251
	i64 u0xb05cc42cd94c6d9d, ; 798: lib-sv-Microsoft.Maui.Controls.resources.dll.so => 370
	i64 u0xb0ac21bec8f428c5, ; 799: Xamarin.AndroidX.Lifecycle.Runtime.Ktx.Android.dll => 277
	i64 u0xb0bb43dc52ea59f9, ; 800: System.Diagnostics.Tracing.dll => 34
	i64 u0xb0d9031058573517, ; 801: Google.Cloud.BigQuery.V2 => 187
	i64 u0xb1dd05401aa8ee63, ; 802: System.Security.AccessControl => 118
	i64 u0xb220631954820169, ; 803: System.Text.RegularExpressions => 139
	i64 u0xb2376e1dbf8b4ed7, ; 804: System.Security.Cryptography.Csp => 122
	i64 u0xb2a1959fe95c5402, ; 805: lib_System.Runtime.InteropServices.JavaScript.dll.so => 106
	i64 u0xb2a3f67f3bf29fce, ; 806: da/Microsoft.Maui.Controls.resources => 347
	i64 u0xb2a49b9ff0535ff2, ; 807: Google.Apis.Bigquery.v2 => 185
	i64 u0xb3011a0a57f7ffb2, ; 808: Microsoft.VisualStudio.DesignTools.MobileTapContracts.dll => 379
	i64 u0xb3874072ee0ecf8c, ; 809: Xamarin.AndroidX.VectorDrawable.Animated.dll => 299
	i64 u0xb39eed1decc0cd95, ; 810: Google.Api.Gax.dll => 180
	i64 u0xb3f0a0fcda8d3ebc, ; 811: Xamarin.AndroidX.CardView => 246
	i64 u0xb404462cdf3bffdb, ; 812: lib_Xamarin.Firebase.ProtoliteWellKnownTypes.dll.so => 310
	i64 u0xb4512edf6d2b372b, ; 813: Google.Cloud.Location => 190
	i64 u0xb46be1aa6d4fff93, ; 814: hi/Microsoft.Maui.Controls.resources => 354
	i64 u0xb477491be13109d8, ; 815: ar/Microsoft.Maui.Controls.resources => 344
	i64 u0xb4bd7015ecee9d86, ; 816: System.IO.Pipelines => 54
	i64 u0xb4c53d9749c5f226, ; 817: lib_System.IO.FileSystem.AccessControl.dll.so => 47
	i64 u0xb4ff710863453fda, ; 818: System.Diagnostics.FileVersionInfo.dll => 28
	i64 u0xb5c38bf497a4cfe2, ; 819: lib_System.Threading.Tasks.dll.so => 145
	i64 u0xb5c7fcdafbc67ee4, ; 820: Microsoft.Extensions.Logging.Abstractions.dll => 210
	i64 u0xb5ea31d5244c6626, ; 821: System.Threading.ThreadPool.dll => 147
	i64 u0xb7212c4683a94afe, ; 822: System.Drawing.Primitives => 35
	i64 u0xb7b7753d1f319409, ; 823: sv/Microsoft.Maui.Controls.resources => 370
	i64 u0xb81a2c6e0aee50fe, ; 824: lib_System.Private.CoreLib.dll.so => 173
	i64 u0xb83d62513b416333, ; 825: ISO3166 => 199
	i64 u0xb872c26142d22aa9, ; 826: Microsoft.Extensions.Http.dll => 208
	i64 u0xb898d1802c1a108c, ; 827: lib_System.Management.dll.so => 230
	i64 u0xb8b0a9b3dfbc5cb7, ; 828: Xamarin.AndroidX.Window.Extensions.Core.Core => 304
	i64 u0xb8c60af47c08d4da, ; 829: System.Net.ServicePoint => 75
	i64 u0xb8e68d20aad91196, ; 830: lib_System.Xml.XPath.dll.so => 161
	i64 u0xb90ff82c284e9af9, ; 831: Grpc.Core.Api => 196
	i64 u0xb9185c33a1643eed, ; 832: Microsoft.CSharp.dll => 1
	i64 u0xb9b19a3eb1924681, ; 833: lib_Microsoft.Maui.Controls.Maps.dll.so => 216
	i64 u0xb9b8001adf4ed7cc, ; 834: lib_Xamarin.AndroidX.SlidingPaneLayout.dll.so => 293
	i64 u0xb9f64d3b230def68, ; 835: lib-pt-Microsoft.Maui.Controls.resources.dll.so => 366
	i64 u0xb9fc3c8a556e3691, ; 836: ja/Microsoft.Maui.Controls.resources => 359
	i64 u0xba4670aa94a2b3c6, ; 837: lib_System.Xml.XDocument.dll.so => 159
	i64 u0xba48785529705af9, ; 838: System.Collections.dll => 12
	i64 u0xba965b8c86359996, ; 839: lib_System.Windows.dll.so => 155
	i64 u0xbb286883bc35db36, ; 840: System.Transactions.dll => 151
	i64 u0xbb6026d73f757bcf, ; 841: Google.Api.Gax.Grpc => 181
	i64 u0xbb65706fde942ce3, ; 842: System.Net.Sockets => 76
	i64 u0xbba28979413cad9e, ; 843: lib_System.Runtime.CompilerServices.VisualC.dll.so => 103
	i64 u0xbbd180354b67271a, ; 844: System.Runtime.Serialization.Formatters => 112
	i64 u0xbc0e640e7c6bcdf8, ; 845: Xamarin.Grpc.Api => 325
	i64 u0xbc260cdba33291a3, ; 846: Xamarin.AndroidX.Arch.Core.Common.dll => 243
	i64 u0xbd0e2c0d55246576, ; 847: System.Net.Http.dll => 65
	i64 u0xbd3fbd85b9e1cb29, ; 848: lib_System.Net.HttpListener.dll.so => 66
	i64 u0xbd437a2cdb333d0d, ; 849: Xamarin.AndroidX.ViewPager2 => 302
	i64 u0xbd4f572d2bd0a789, ; 850: System.IO.Compression.ZipFile.dll => 45
	i64 u0xbd5d0b88d3d647a5, ; 851: lib_Xamarin.AndroidX.Browser.dll.so => 245
	i64 u0xbd854cde2dba71a3, ; 852: lib_Square.OkHttp.dll.so => 226
	i64 u0xbd877b14d0b56392, ; 853: System.Runtime.Intrinsics.dll => 109
	i64 u0xbe65a49036345cf4, ; 854: lib_System.Buffers.dll.so => 7
	i64 u0xbee38d4a88835966, ; 855: Xamarin.AndroidX.AppCompat.AppCompatResources => 242
	i64 u0xbef9919db45b4ca7, ; 856: System.IO.Pipes.AccessControl => 55
	i64 u0xbf0fa68611139208, ; 857: lib_Xamarin.AndroidX.Annotation.dll.so => 238
	i64 u0xbfc1e1fb3095f2b3, ; 858: lib_System.Net.Http.Json.dll.so => 64
	i64 u0xc040a4ab55817f58, ; 859: ar/Microsoft.Maui.Controls.resources.dll => 344
	i64 u0xc07cadab29efeba0, ; 860: Xamarin.AndroidX.Core.Core.Ktx.dll => 255
	i64 u0xc0d928351ab5ca77, ; 861: System.Console.dll => 20
	i64 u0xc0f5a221a9383aea, ; 862: System.Runtime.Intrinsics => 109
	i64 u0xc111030af54d7191, ; 863: System.Resources.Writer => 101
	i64 u0xc12b8b3afa48329c, ; 864: lib_System.Linq.dll.so => 62
	i64 u0xc1649f545b2f76aa, ; 865: Grpc.Auth => 195
	i64 u0xc183ca0b74453aa9, ; 866: lib_System.Threading.Tasks.Dataflow.dll.so => 142
	i64 u0xc1ff9ae3cdb6e1e6, ; 867: Xamarin.AndroidX.Activity.dll => 236
	i64 u0xc226d517e7e30388, ; 868: lib_Xamarin.Grpc.Stub.dll.so => 330
	i64 u0xc26c064effb1dea9, ; 869: System.Buffers.dll => 7
	i64 u0xc2850fbba221599d, ; 870: lib_Google.Apis.Core.dll.so => 186
	i64 u0xc28c50f32f81cc73, ; 871: ja/Microsoft.Maui.Controls.resources.dll => 359
	i64 u0xc2902f6cf5452577, ; 872: lib_Mono.Android.Export.dll.so => 170
	i64 u0xc2a3bca55b573141, ; 873: System.IO.FileSystem.Watcher => 50
	i64 u0xc2bcfec99f69365e, ; 874: Xamarin.AndroidX.ViewPager2.dll => 302
	i64 u0xc30b52815b58ac2c, ; 875: lib_System.Runtime.Serialization.Xml.dll.so => 115
	i64 u0xc36d7d89c652f455, ; 876: System.Threading.Overlapped => 141
	i64 u0xc396b285e59e5493, ; 877: GoogleGson.dll => 194
	i64 u0xc3c86c1e5e12f03d, ; 878: WindowsBase => 166
	i64 u0xc421b61fd853169d, ; 879: lib_System.Net.WebSockets.Client.dll.so => 80
	i64 u0xc463e077917aa21d, ; 880: System.Runtime.Serialization.Json => 113
	i64 u0xc4d3858ed4d08512, ; 881: Xamarin.AndroidX.Lifecycle.ViewModelSavedState.dll => 281
	i64 u0xc50fded0ded1418c, ; 882: lib_System.ComponentModel.TypeConverter.dll.so => 17
	i64 u0xc519125d6bc8fb11, ; 883: lib_System.Net.Requests.dll.so => 73
	i64 u0xc5293b19e4dc230e, ; 884: Xamarin.AndroidX.Navigation.Fragment => 284
	i64 u0xc5325b2fcb37446f, ; 885: lib_System.Private.Xml.dll.so => 89
	i64 u0xc535cb9a21385d9b, ; 886: lib_Xamarin.Android.Glide.DiskLruCache.dll.so => 234
	i64 u0xc5a0f4b95a699af7, ; 887: lib_System.Private.Uri.dll.so => 87
	i64 u0xc5cdcd5b6277579e, ; 888: lib_System.Security.Cryptography.Algorithms.dll.so => 120
	i64 u0xc5d608afb58abba2, ; 889: Google.Apis.Auth.dll => 184
	i64 u0xc5ec286825cb0bf4, ; 890: Xamarin.AndroidX.Tracing.Tracing => 296
	i64 u0xc5fe73d2394f68ac, ; 891: Xamarin.Io.OpenCensus.OpenCensusApi.dll => 331
	i64 u0xc62af3e2d6d38289, ; 892: lib_Xamarin.Firebase.Firestore.dll.so => 309
	i64 u0xc64f6952cef5d09f, ; 893: Microsoft.Maui.Maps.dll => 221
	i64 u0xc6706bc8aa7fe265, ; 894: Xamarin.AndroidX.Annotation.Jvm => 240
	i64 u0xc68e480c8069e1f7, ; 895: Microsoft.Maui.Maps => 221
	i64 u0xc6e86f9bc00a35c2, ; 896: lib_Google.Cloud.BigQuery.V2.dll.so => 187
	i64 u0xc715c0dc971dd04e, ; 897: CFAN.SchoolMap.Maui => 0
	i64 u0xc7c01e7d7c93a110, ; 898: System.Text.Encoding.Extensions.dll => 135
	i64 u0xc7ce851898a4548e, ; 899: lib_System.Web.HttpUtility.dll.so => 153
	i64 u0xc809d4089d2556b2, ; 900: System.Runtime.InteropServices.JavaScript.dll => 106
	i64 u0xc858a28d9ee5a6c5, ; 901: lib_System.Collections.Specialized.dll.so => 11
	i64 u0xc8ac7c6bf1c2ec51, ; 902: System.Reflection.DispatchProxy.dll => 90
	i64 u0xc9c62c8f354ac568, ; 903: lib_System.Diagnostics.TextWriterTraceListener.dll.so => 31
	i64 u0xca207bdbc4085222, ; 904: Acr.UserDialogs.Maui => 175
	i64 u0xca3a723e7342c5b6, ; 905: lib-tr-Microsoft.Maui.Controls.resources.dll.so => 372
	i64 u0xca5801070d9fccfb, ; 906: System.Text.Encoding => 136
	i64 u0xcab3493c70141c2d, ; 907: pl/Microsoft.Maui.Controls.resources => 364
	i64 u0xcab69b9a31439815, ; 908: lib_Xamarin.Google.ErrorProne.TypeAnnotations.dll.so => 316
	i64 u0xcacfddc9f7c6de76, ; 909: ro/Microsoft.Maui.Controls.resources.dll => 367
	i64 u0xcadbc92899a777f0, ; 910: Xamarin.AndroidX.Startup.StartupRuntime => 294
	i64 u0xcb76efab0f56f81a, ; 911: System.Reactive => 231
	i64 u0xcba1cb79f45292b5, ; 912: Xamarin.Android.Glide.GifDecoder.dll => 235
	i64 u0xcbb5f80c7293e696, ; 913: lib_System.Globalization.Calendars.dll.so => 40
	i64 u0xcbd4fdd9cef4a294, ; 914: lib__Microsoft.Android.Resource.Designer.dll.so => 382
	i64 u0xcc15da1e07bbd994, ; 915: Xamarin.AndroidX.SlidingPaneLayout => 293
	i64 u0xcc182c3afdc374d6, ; 916: Microsoft.Bcl.AsyncInterfaces => 200
	i64 u0xcc2876b32ef2794c, ; 917: lib_System.Text.RegularExpressions.dll.so => 139
	i64 u0xcc5c3bb714c4561e, ; 918: Xamarin.KotlinX.Coroutines.Core.Jvm.dll => 340
	i64 u0xcc76886e09b88260, ; 919: Xamarin.KotlinX.Serialization.Core.Jvm.dll => 342
	i64 u0xcc9fa2923aa1c9ef, ; 920: System.Diagnostics.Contracts.dll => 25
	i64 u0xccf25c4b634ccd3a, ; 921: zh-Hans/Microsoft.Maui.Controls.resources.dll => 376
	i64 u0xcd10a42808629144, ; 922: System.Net.Requests => 73
	i64 u0xcdca1b920e9f53ba, ; 923: Xamarin.AndroidX.Interpolator => 267
	i64 u0xcdd0c48b6937b21c, ; 924: Xamarin.AndroidX.SwipeRefreshLayout => 295
	i64 u0xcde1fa22dc303670, ; 925: Microsoft.VisualStudio.DesignTools.XamlTapContract => 381
	i64 u0xcf23d8093f3ceadf, ; 926: System.Diagnostics.DiagnosticSource.dll => 27
	i64 u0xcf5ff6b6b2c4c382, ; 927: System.Net.Mail.dll => 67
	i64 u0xcf8fc898f98b0d34, ; 928: System.Private.Xml.Linq => 88
	i64 u0xcfb21487d9cb358b, ; 929: Xamarin.GooglePlayServices.Maps.dll => 322
	i64 u0xcfe5335ac6c8a61c, ; 930: Acr.UserDialogs => 174
	i64 u0xd04b5f59ed596e31, ; 931: System.Reflection.Metadata.dll => 95
	i64 u0xd063299fcfc0c93f, ; 932: lib_System.Runtime.Serialization.Json.dll.so => 113
	i64 u0xd0de8a113e976700, ; 933: System.Diagnostics.TextWriterTraceListener => 31
	i64 u0xd0fc33d5ae5d4cb8, ; 934: System.Runtime.Extensions => 104
	i64 u0xd1194e1d8a8de83c, ; 935: lib_Xamarin.AndroidX.Lifecycle.Common.Jvm.dll.so => 269
	i64 u0xd12beacdfc14f696, ; 936: System.Dynamic.Runtime => 37
	i64 u0xd16fd7fb9bbcd43e, ; 937: Microsoft.Extensions.Diagnostics.Abstractions => 207
	i64 u0xd198e7ce1b6a8344, ; 938: System.Net.Quic.dll => 72
	i64 u0xd20588bdafd1c17c, ; 939: Xamarin.Grpc.Stub.dll => 330
	i64 u0xd3144156a3727ebe, ; 940: Xamarin.Google.Guava.ListenableFuture => 319
	i64 u0xd333d0af9e423810, ; 941: System.Runtime.InteropServices => 108
	i64 u0xd33a415cb4278969, ; 942: System.Security.Cryptography.Encoding.dll => 123
	i64 u0xd3426d966bb704f5, ; 943: Xamarin.AndroidX.AppCompat.AppCompatResources.dll => 242
	i64 u0xd3651b6fc3125825, ; 944: System.Private.Uri.dll => 87
	i64 u0xd373685349b1fe8b, ; 945: Microsoft.Extensions.Logging.dll => 209
	i64 u0xd3801faafafb7698, ; 946: System.Private.DataContractSerialization.dll => 86
	i64 u0xd3e4c8d6a2d5d470, ; 947: it/Microsoft.Maui.Controls.resources => 358
	i64 u0xd3edcc1f25459a50, ; 948: System.Reflection.Emit => 93
	i64 u0xd4645626dffec99d, ; 949: lib_Microsoft.Extensions.DependencyInjection.Abstractions.dll.so => 205
	i64 u0xd4fa0abb79079ea9, ; 950: System.Security.Principal.dll => 129
	i64 u0xd5507e11a2b2839f, ; 951: Xamarin.AndroidX.Lifecycle.ViewModelSavedState => 281
	i64 u0xd587ccfff9fdf214, ; 952: GeoJSON.Net.dll => 178
	i64 u0xd5d04bef8478ea19, ; 953: Xamarin.AndroidX.Tracing.Tracing.dll => 296
	i64 u0xd60815f26a12e140, ; 954: Microsoft.Extensions.Logging.Debug.dll => 211
	i64 u0xd64f50eb4ba264b3, ; 955: lib_Google.LongRunning.dll.so => 191
	i64 u0xd65786d27a4ad960, ; 956: lib_Microsoft.Maui.Controls.HotReload.Forms.dll.so => 378
	i64 u0xd6694f8359737e4e, ; 957: Xamarin.AndroidX.SavedState => 290
	i64 u0xd6949e129339eae5, ; 958: lib_Xamarin.AndroidX.Core.Core.Ktx.dll.so => 255
	i64 u0xd6d21782156bc35b, ; 959: Xamarin.AndroidX.SwipeRefreshLayout.dll => 295
	i64 u0xd6de019f6af72435, ; 960: Xamarin.AndroidX.ConstraintLayout.Core.dll => 252
	i64 u0xd6ed09ee80649430, ; 961: lib_Xamarin.Grpc.Core.dll.so => 327
	i64 u0xd6f697a581fc6fe3, ; 962: Xamarin.Google.ErrorProne.TypeAnnotations.dll => 316
	i64 u0xd70956d1e6deefb9, ; 963: Jsr305Binding => 313
	i64 u0xd72329819cbbbc44, ; 964: lib_Microsoft.Extensions.Configuration.Abstractions.dll.so => 202
	i64 u0xd72c760af136e863, ; 965: System.Xml.XmlSerializer.dll => 163
	i64 u0xd753f071e44c2a03, ; 966: lib_System.Security.SecureString.dll.so => 130
	i64 u0xd7b3764ada9d341d, ; 967: lib_Microsoft.Extensions.Logging.Abstractions.dll.so => 210
	i64 u0xd7dfd89d34e8dd1d, ; 968: Square.OkIO => 227
	i64 u0xd7f0088bc5ad71f2, ; 969: Xamarin.AndroidX.VersionedParcelable => 300
	i64 u0xd8113d9a7e8ad136, ; 970: System.CodeDom => 228
	i64 u0xd8fb25e28ae30a12, ; 971: Xamarin.AndroidX.ProfileInstaller.ProfileInstaller.dll => 287
	i64 u0xd908c01a3824c19c, ; 972: lib_Acr.UserDialogs.dll.so => 174
	i64 u0xda1dfa4c534a9251, ; 973: Microsoft.Extensions.DependencyInjection => 204
	i64 u0xdad05a11827959a3, ; 974: System.Collections.NonGeneric.dll => 10
	i64 u0xdaefdfe71aa53cf9, ; 975: System.IO.FileSystem.Primitives => 49
	i64 u0xdaff1e02a729f3a2, ; 976: Xamarin.Grpc.Android => 324
	i64 u0xdb5383ab5865c007, ; 977: lib-vi-Microsoft.Maui.Controls.resources.dll.so => 374
	i64 u0xdb58816721c02a59, ; 978: lib_System.Reflection.Emit.ILGeneration.dll.so => 91
	i64 u0xdbeda89f832aa805, ; 979: vi/Microsoft.Maui.Controls.resources.dll => 374
	i64 u0xdbf2a779fbc3ac31, ; 980: System.Transactions.Local.dll => 150
	i64 u0xdbf9607a441b4505, ; 981: System.Linq => 62
	i64 u0xdbfc90157a0de9b0, ; 982: lib_System.Text.Encoding.dll.so => 136
	i64 u0xdc75032002d1a212, ; 983: lib_System.Transactions.Local.dll.so => 150
	i64 u0xdca8be7403f92d4f, ; 984: lib_System.Linq.Queryable.dll.so => 61
	i64 u0xdcbd21904ff0f297, ; 985: Google.Apis => 183
	i64 u0xdce2c53525640bf3, ; 986: Microsoft.Extensions.Logging => 209
	i64 u0xdd2b722d78ef5f43, ; 987: System.Runtime.dll => 117
	i64 u0xdd67031857c72f96, ; 988: lib_System.Text.Encodings.Web.dll.so => 137
	i64 u0xdd70765ad6162057, ; 989: Xamarin.JSpecify => 334
	i64 u0xdd92e229ad292030, ; 990: System.Numerics.dll => 84
	i64 u0xdde30e6b77aa6f6c, ; 991: lib-zh-Hans-Microsoft.Maui.Controls.resources.dll.so => 376
	i64 u0xde110ae80fa7c2e2, ; 992: System.Xml.XDocument.dll => 159
	i64 u0xde4726fcdf63a198, ; 993: Xamarin.AndroidX.Transition => 297
	i64 u0xde572c2b2fb32f93, ; 994: lib_System.Threading.Tasks.Extensions.dll.so => 143
	i64 u0xde8769ebda7d8647, ; 995: hr/Microsoft.Maui.Controls.resources.dll => 355
	i64 u0xdee075f3477ef6be, ; 996: Xamarin.AndroidX.ExifInterface.dll => 264
	i64 u0xdf4b773de8fb1540, ; 997: System.Net.dll => 82
	i64 u0xdfa254ebb4346068, ; 998: System.Net.Ping => 70
	i64 u0xe0142572c095a480, ; 999: Xamarin.AndroidX.AppCompat.dll => 241
	i64 u0xe021eaa401792a05, ; 1000: System.Text.Encoding.dll => 136
	i64 u0xe02f89350ec78051, ; 1001: Xamarin.AndroidX.CoordinatorLayout.dll => 253
	i64 u0xe0496b9d65ef5474, ; 1002: Xamarin.Android.Glide.DiskLruCache.dll => 234
	i64 u0xe10b760bb1462e7a, ; 1003: lib_System.Security.Cryptography.Primitives.dll.so => 125
	i64 u0xe1566bbdb759c5af, ; 1004: Microsoft.Maui.Controls.HotReload.Forms.dll => 378
	i64 u0xe192a588d4410686, ; 1005: lib_System.IO.Pipelines.dll.so => 54
	i64 u0xe1a08bd3fa539e0d, ; 1006: System.Runtime.Loader => 110
	i64 u0xe1a77eb8831f7741, ; 1007: System.Security.SecureString.dll => 130
	i64 u0xe1b52f9f816c70ef, ; 1008: System.Private.Xml.Linq.dll => 88
	i64 u0xe1d23f7f2e51bed4, ; 1009: AndHUD.dll => 176
	i64 u0xe1e199c8ab02e356, ; 1010: System.Data.DataSetExtensions.dll => 23
	i64 u0xe1ecfdb7fff86067, ; 1011: System.Net.Security.dll => 74
	i64 u0xe2252a80fe853de4, ; 1012: lib_System.Security.Principal.dll.so => 129
	i64 u0xe22fa4c9c645db62, ; 1013: System.Diagnostics.TextWriterTraceListener.dll => 31
	i64 u0xe2420585aeceb728, ; 1014: System.Net.Requests.dll => 73
	i64 u0xe26692647e6bcb62, ; 1015: Xamarin.AndroidX.Lifecycle.Runtime.Ktx => 276
	i64 u0xe29b73bc11392966, ; 1016: lib-id-Microsoft.Maui.Controls.resources.dll.so => 357
	i64 u0xe2ad448dee50fbdf, ; 1017: System.Xml.Serialization => 158
	i64 u0xe2d920f978f5d85c, ; 1018: System.Data.DataSetExtensions => 23
	i64 u0xe2e426c7714fa0bc, ; 1019: Microsoft.Win32.Primitives.dll => 4
	i64 u0xe32760cdfe9553f7, ; 1020: OpenLocationCode.dll => 224
	i64 u0xe332bacb3eb4a806, ; 1021: Mono.Android.Export.dll => 170
	i64 u0xe3811d68d4fe8463, ; 1022: pt-BR/Microsoft.Maui.Controls.resources.dll => 365
	i64 u0xe3b7cbae5ad66c75, ; 1023: lib_System.Security.Cryptography.Encoding.dll.so => 123
	i64 u0xe4292b48f3224d5b, ; 1024: lib_Xamarin.AndroidX.Core.ViewTree.dll.so => 256
	i64 u0xe494f7ced4ecd10a, ; 1025: hu/Microsoft.Maui.Controls.resources.dll => 356
	i64 u0xe49a982a2533a332, ; 1026: lib_Google.Cloud.Location.dll.so => 190
	i64 u0xe4a9b1e40d1e8917, ; 1027: lib-fi-Microsoft.Maui.Controls.resources.dll.so => 351
	i64 u0xe4f74a0b5bf9703f, ; 1028: System.Runtime.Serialization.Primitives => 114
	i64 u0xe5434e8a119ceb69, ; 1029: lib_Mono.Android.dll.so => 172
	i64 u0xe55703b9ce5c038a, ; 1030: System.Diagnostics.Tools => 32
	i64 u0xe57013c8afc270b5, ; 1031: Microsoft.VisualBasic => 3
	i64 u0xe62913cc36bc07ec, ; 1032: System.Xml.dll => 164
	i64 u0xe6e77c648688b75b, ; 1033: Google.Api.CommonProtos.dll => 179
	i64 u0xe706def48e047d1a, ; 1034: GoogleApi => 193
	i64 u0xe7b0691bcbb5a85d, ; 1035: System.Linq.Async => 229
	i64 u0xe7bea09c4900a191, ; 1036: Xamarin.AndroidX.VectorDrawable.dll => 298
	i64 u0xe7e03cc18dcdeb49, ; 1037: lib_System.Diagnostics.StackTrace.dll.so => 30
	i64 u0xe7e147ff99a7a380, ; 1038: lib_System.Configuration.dll.so => 19
	i64 u0xe8397cf3948e7cb7, ; 1039: lib_Microsoft.Extensions.Options.ConfigurationExtensions.dll.so => 213
	i64 u0xe86b0df4ba9e5db8, ; 1040: lib_Xamarin.AndroidX.Lifecycle.Runtime.Android.dll.so => 275
	i64 u0xe896622fe0902957, ; 1041: System.Reflection.Emit.dll => 93
	i64 u0xe89a2a9ef110899b, ; 1042: System.Drawing.dll => 36
	i64 u0xe8c5f8c100b5934b, ; 1043: Microsoft.Win32.Registry => 5
	i64 u0xe8efe6c2171f7cd2, ; 1044: Xamarin.Google.Guava.dll => 317
	i64 u0xe93ca41931f1f2d0, ; 1045: Xamarin.Grpc.Api.dll => 325
	i64 u0xe957c3976986ab72, ; 1046: lib_Xamarin.AndroidX.Window.Extensions.Core.Core.dll.so => 304
	i64 u0xe98163eb702ae5c5, ; 1047: Xamarin.AndroidX.Arch.Core.Runtime => 244
	i64 u0xe98b0e4b4d44e931, ; 1048: lib_Grpc.Net.Client.dll.so => 197
	i64 u0xe994f23ba4c143e5, ; 1049: Xamarin.KotlinX.Coroutines.Android => 338
	i64 u0xe9b9c8c0458fd92a, ; 1050: System.Windows => 155
	i64 u0xe9d166d87a7f2bdb, ; 1051: lib_Xamarin.AndroidX.Startup.StartupRuntime.dll.so => 294
	i64 u0xea5a4efc2ad81d1b, ; 1052: Xamarin.Google.ErrorProne.Annotations => 315
	i64 u0xeaf8e9970fc2fe69, ; 1053: System.Management => 230
	i64 u0xeb2313fe9d65b785, ; 1054: Xamarin.AndroidX.ConstraintLayout.dll => 251
	i64 u0xeb9973cda26e858f, ; 1055: Xamarin.Firebase.Common.dll => 306
	i64 u0xebb38f64b9e27ecc, ; 1056: OpenLocationCode => 224
	i64 u0xecc466205a3a680e, ; 1057: Google.Cloud.BigQuery.V2.dll => 187
	i64 u0xed19c616b3fcb7eb, ; 1058: Xamarin.AndroidX.VersionedParcelable.dll => 300
	i64 u0xed60c6fa891c051a, ; 1059: lib_Microsoft.VisualStudio.DesignTools.TapContract.dll.so => 380
	i64 u0xedc4817167106c23, ; 1060: System.Net.Sockets.dll => 76
	i64 u0xedc632067fb20ff3, ; 1061: System.Memory.dll => 63
	i64 u0xedc8e4ca71a02a8b, ; 1062: Xamarin.AndroidX.Navigation.Runtime.dll => 285
	i64 u0xee27c952ed6d058b, ; 1063: Microsoft.Maui.Controls.Maps => 216
	i64 u0xee81f5b3f1c4f83b, ; 1064: System.Threading.ThreadPool => 147
	i64 u0xeeb7ebb80150501b, ; 1065: lib_Xamarin.AndroidX.Collection.Jvm.dll.so => 248
	i64 u0xeefc635595ef57f0, ; 1066: System.Security.Cryptography.Cng => 121
	i64 u0xef03b1b5a04e9709, ; 1067: System.Text.Encoding.CodePages.dll => 134
	i64 u0xef602c523fe2e87a, ; 1068: lib_Xamarin.Google.Guava.ListenableFuture.dll.so => 319
	i64 u0xef72742e1bcca27a, ; 1069: Microsoft.Maui.Essentials.dll => 219
	i64 u0xefd1e0c4e5c9b371, ; 1070: System.Resources.ResourceManager.dll => 100
	i64 u0xefe8f8d5ed3c72ea, ; 1071: System.Formats.Tar.dll => 39
	i64 u0xefec0b7fdc57ec42, ; 1072: Xamarin.AndroidX.Activity => 236
	i64 u0xf008bcd238ede2c8, ; 1073: System.CodeDom.dll => 228
	i64 u0xf00c29406ea45e19, ; 1074: es/Microsoft.Maui.Controls.resources.dll => 350
	i64 u0xf09e47b6ae914f6e, ; 1075: System.Net.NameResolution => 68
	i64 u0xf0ac2b489fed2e35, ; 1076: lib_System.Diagnostics.Debug.dll.so => 26
	i64 u0xf0bb49dadd3a1fe1, ; 1077: lib_System.Net.ServicePoint.dll.so => 75
	i64 u0xf0de2537ee19c6ca, ; 1078: lib_System.Net.WebHeaderCollection.dll.so => 78
	i64 u0xf1138779fa181c68, ; 1079: lib_Xamarin.AndroidX.Lifecycle.Runtime.dll.so => 274
	i64 u0xf11b621fc87b983f, ; 1080: Microsoft.Maui.Controls.Xaml.dll => 217
	i64 u0xf161f4f3c3b7e62c, ; 1081: System.Data => 24
	i64 u0xf16eb650d5a464bc, ; 1082: System.ValueTuple => 152
	i64 u0xf1c4b4005493d871, ; 1083: System.Formats.Asn1.dll => 38
	i64 u0xf238bd79489d3a96, ; 1084: lib-nl-Microsoft.Maui.Controls.resources.dll.so => 363
	i64 u0xf2feea356ba760af, ; 1085: Xamarin.AndroidX.Arch.Core.Runtime.dll => 244
	i64 u0xf300e085f8acd238, ; 1086: lib_System.ServiceProcess.dll.so => 133
	i64 u0xf34e52b26e7e059d, ; 1087: System.Runtime.CompilerServices.VisualC.dll => 103
	i64 u0xf37221fda4ef8830, ; 1088: lib_Xamarin.Google.Android.Material.dll.so => 311
	i64 u0xf3ad9b8fb3eefd12, ; 1089: lib_System.IO.UnmanagedMemoryStream.dll.so => 57
	i64 u0xf3ddfe05336abf29, ; 1090: System => 165
	i64 u0xf408654b2a135055, ; 1091: System.Reflection.Emit.ILGeneration.dll => 91
	i64 u0xf4103170a1de5bd0, ; 1092: System.Linq.Queryable.dll => 61
	i64 u0xf42d20c23173d77c, ; 1093: lib_System.ServiceModel.Web.dll.so => 132
	i64 u0xf483be3bba89b4ff, ; 1094: lib_Xamarin.Grpc.Api.dll.so => 325
	i64 u0xf4c1dd70a5496a17, ; 1095: System.IO.Compression => 46
	i64 u0xf4ecf4b9afc64781, ; 1096: System.ServiceProcess.dll => 133
	i64 u0xf4eeeaa566e9b970, ; 1097: lib_Xamarin.AndroidX.CustomView.PoolingContainer.dll.so => 259
	i64 u0xf518f63ead11fcd1, ; 1098: System.Threading.Tasks => 145
	i64 u0xf5fc7602fe27b333, ; 1099: System.Net.WebHeaderCollection => 78
	i64 u0xf6077741019d7428, ; 1100: Xamarin.AndroidX.CoordinatorLayout => 253
	i64 u0xf6742cbf457c450b, ; 1101: Xamarin.AndroidX.Lifecycle.Runtime.Android.dll => 275
	i64 u0xf6f893f692f8cb43, ; 1102: Microsoft.Extensions.Options.ConfigurationExtensions.dll => 213
	i64 u0xf70c0a7bf8ccf5af, ; 1103: System.Web => 154
	i64 u0xf73b506af603bfe1, ; 1104: lib_Square.OkIO.dll.so => 227
	i64 u0xf77b20923f07c667, ; 1105: de/Microsoft.Maui.Controls.resources.dll => 348
	i64 u0xf7e2cac4c45067b3, ; 1106: lib_System.Numerics.Vectors.dll.so => 83
	i64 u0xf7e74930e0e3d214, ; 1107: zh-HK/Microsoft.Maui.Controls.resources.dll => 375
	i64 u0xf7fa0bf77fe677cc, ; 1108: Newtonsoft.Json.dll => 222
	i64 u0xf84773b5c81e3cef, ; 1109: lib-uk-Microsoft.Maui.Controls.resources.dll.so => 373
	i64 u0xf8aac5ea82de1348, ; 1110: System.Linq.Queryable => 61
	i64 u0xf8b77539b362d3ba, ; 1111: lib_System.Reflection.Primitives.dll.so => 96
	i64 u0xf8dacc6dd9573437, ; 1112: Square.OkIO.dll => 227
	i64 u0xf8e045dc345b2ea3, ; 1113: lib_Xamarin.AndroidX.RecyclerView.dll.so => 288
	i64 u0xf8f4145fd91f42a5, ; 1114: lib_GeoJSON.Net.dll.so => 178
	i64 u0xf915dc29808193a1, ; 1115: System.Web.HttpUtility.dll => 153
	i64 u0xf96c777a2a0686f4, ; 1116: hi/Microsoft.Maui.Controls.resources.dll => 354
	i64 u0xf9be54c8bcf8ff3b, ; 1117: System.Security.AccessControl.dll => 118
	i64 u0xf9eec5bb3a6aedc6, ; 1118: Microsoft.Extensions.Options => 212
	i64 u0xf9f8e26574f88818, ; 1119: GoogleApi.dll => 193
	i64 u0xfa0e82300e67f913, ; 1120: lib_System.AppContext.dll.so => 6
	i64 u0xfa2fdb27e8a2c8e8, ; 1121: System.ComponentModel.EventBasedAsync => 15
	i64 u0xfa3f278f288b0e84, ; 1122: lib_System.Net.Security.dll.so => 74
	i64 u0xfa5ed7226d978949, ; 1123: lib-ar-Microsoft.Maui.Controls.resources.dll.so => 344
	i64 u0xfa645d91e9fc4cba, ; 1124: System.Threading.Thread => 146
	i64 u0xfad4d2c770e827f9, ; 1125: lib_System.IO.IsolatedStorage.dll.so => 52
	i64 u0xfb06dd2338e6f7c4, ; 1126: System.Net.Ping.dll => 70
	i64 u0xfb087abe5365e3b7, ; 1127: lib_System.Data.DataSetExtensions.dll.so => 23
	i64 u0xfb846e949baff5ea, ; 1128: System.Xml.Serialization.dll => 158
	i64 u0xfbad3e4ce4b98145, ; 1129: System.Security.Cryptography.X509Certificates => 126
	i64 u0xfbf0a31c9fc34bc4, ; 1130: lib_System.Net.Http.dll.so => 65
	i64 u0xfc6b7527cc280b3f, ; 1131: lib_System.Runtime.Serialization.Formatters.dll.so => 112
	i64 u0xfc719aec26adf9d9, ; 1132: Xamarin.AndroidX.Navigation.Fragment.dll => 284
	i64 u0xfc82690c2fe2735c, ; 1133: Xamarin.AndroidX.Lifecycle.Process.dll => 273
	i64 u0xfc93fc307d279893, ; 1134: System.IO.Pipes.AccessControl.dll => 55
	i64 u0xfcd302092ada6328, ; 1135: System.IO.MemoryMappedFiles.dll => 53
	i64 u0xfd22f00870e40ae0, ; 1136: lib_Xamarin.AndroidX.DrawerLayout.dll.so => 260
	i64 u0xfd3ce7bc9232d417, ; 1137: Xamarin.Firebase.Firestore.dll => 309
	i64 u0xfd49b3c1a76e2748, ; 1138: System.Runtime.InteropServices.RuntimeInformation => 107
	i64 u0xfd536c702f64dc47, ; 1139: System.Text.Encoding.Extensions => 135
	i64 u0xfd583f7657b6a1cb, ; 1140: Xamarin.AndroidX.Fragment => 265
	i64 u0xfd8dd91a2c26bd5d, ; 1141: Xamarin.AndroidX.Lifecycle.Runtime => 274
	i64 u0xfda36abccf05cf5c, ; 1142: System.Net.WebSockets.Client => 80
	i64 u0xfddbe9695626a7f5, ; 1143: Xamarin.AndroidX.Lifecycle.Common => 268
	i64 u0xfeae9952cf03b8cb, ; 1144: tr/Microsoft.Maui.Controls.resources => 372
	i64 u0xfebe1950717515f9, ; 1145: Xamarin.AndroidX.Lifecycle.LiveData.Core.Ktx.dll => 272
	i64 u0xff270a55858bac8d, ; 1146: System.Security.Principal => 129
	i64 u0xff9b54613e0d2cc8, ; 1147: System.Net.Http.Json => 64
	i64 u0xffdb7a971be4ec73 ; 1148: System.ValueTuple.dll => 152
], align 8

@assembly_image_cache_indices = dso_local local_unnamed_addr constant [1149 x i32] [
	i32 189, i32 42, i32 339, i32 295, i32 13, i32 197, i32 285, i32 213,
	i32 105, i32 171, i32 48, i32 241, i32 7, i32 199, i32 86, i32 368,
	i32 346, i32 374, i32 261, i32 326, i32 71, i32 323, i32 288, i32 12,
	i32 218, i32 102, i32 375, i32 156, i32 19, i32 193, i32 266, i32 248,
	i32 161, i32 263, i32 298, i32 167, i32 368, i32 10, i32 211, i32 299,
	i32 223, i32 96, i32 259, i32 260, i32 13, i32 212, i32 10, i32 321,
	i32 127, i32 307, i32 95, i32 140, i32 39, i32 369, i32 342, i32 301,
	i32 228, i32 365, i32 322, i32 172, i32 235, i32 5, i32 219, i32 67,
	i32 292, i32 130, i32 200, i32 196, i32 291, i32 262, i32 68, i32 249,
	i32 66, i32 57, i32 200, i32 258, i32 52, i32 43, i32 125, i32 174,
	i32 67, i32 81, i32 276, i32 380, i32 158, i32 92, i32 182, i32 99,
	i32 288, i32 141, i32 189, i32 151, i32 245, i32 352, i32 162, i32 169,
	i32 353, i32 205, i32 81, i32 380, i32 334, i32 249, i32 4, i32 5,
	i32 51, i32 101, i32 330, i32 56, i32 331, i32 120, i32 98, i32 168,
	i32 118, i32 339, i32 21, i32 185, i32 356, i32 137, i32 97, i32 342,
	i32 77, i32 362, i32 294, i32 119, i32 224, i32 229, i32 8, i32 165,
	i32 371, i32 70, i32 234, i32 277, i32 289, i32 206, i32 171, i32 184,
	i32 145, i32 40, i32 292, i32 47, i32 192, i32 30, i32 318, i32 286,
	i32 178, i32 360, i32 144, i32 212, i32 163, i32 28, i32 84, i32 296,
	i32 77, i32 43, i32 180, i32 29, i32 42, i32 103, i32 117, i32 329,
	i32 239, i32 45, i32 91, i32 371, i32 56, i32 148, i32 379, i32 321,
	i32 146, i32 100, i32 49, i32 180, i32 20, i32 254, i32 114, i32 308,
	i32 195, i32 184, i32 232, i32 352, i32 314, i32 335, i32 214, i32 309,
	i32 94, i32 58, i32 357, i32 355, i32 81, i32 314, i32 169, i32 26,
	i32 324, i32 71, i32 287, i32 207, i32 264, i32 378, i32 373, i32 69,
	i32 33, i32 351, i32 14, i32 139, i32 38, i32 331, i32 377, i32 250,
	i32 364, i32 134, i32 92, i32 88, i32 216, i32 149, i32 318, i32 370,
	i32 24, i32 308, i32 138, i32 57, i32 51, i32 349, i32 0, i32 29,
	i32 157, i32 34, i32 164, i32 308, i32 208, i32 265, i32 221, i32 52,
	i32 382, i32 303, i32 90, i32 316, i32 246, i32 35, i32 352, i32 157,
	i32 176, i32 9, i32 350, i32 190, i32 76, i32 327, i32 55, i32 218,
	i32 346, i32 343, i32 215, i32 13, i32 302, i32 201, i32 243, i32 109,
	i32 329, i32 280, i32 32, i32 307, i32 104, i32 84, i32 92, i32 53,
	i32 96, i32 333, i32 181, i32 58, i32 9, i32 102, i32 258, i32 68,
	i32 301, i32 231, i32 345, i32 231, i32 222, i32 125, i32 289, i32 116,
	i32 135, i32 126, i32 106, i32 191, i32 335, i32 131, i32 328, i32 305,
	i32 245, i32 319, i32 147, i32 156, i32 266, i32 254, i32 192, i32 261,
	i32 191, i32 289, i32 97, i32 24, i32 293, i32 143, i32 283, i32 3,
	i32 181, i32 167, i32 242, i32 100, i32 161, i32 188, i32 99, i32 179,
	i32 256, i32 25, i32 230, i32 93, i32 168, i32 172, i32 237, i32 3,
	i32 364, i32 263, i32 1, i32 114, i32 335, i32 266, i32 273, i32 183,
	i32 33, i32 6, i32 368, i32 332, i32 156, i32 198, i32 223, i32 366,
	i32 53, i32 85, i32 306, i32 300, i32 343, i32 286, i32 44, i32 272,
	i32 104, i32 47, i32 138, i32 64, i32 322, i32 282, i32 69, i32 80,
	i32 182, i32 59, i32 89, i32 154, i32 243, i32 133, i32 110, i32 358,
	i32 282, i32 287, i32 171, i32 134, i32 140, i32 40, i32 345, i32 203,
	i32 324, i32 215, i32 312, i32 198, i32 60, i32 177, i32 203, i32 279,
	i32 305, i32 79, i32 25, i32 36, i32 99, i32 276, i32 71, i32 198,
	i32 22, i32 254, i32 220, i32 369, i32 121, i32 69, i32 107, i32 375,
	i32 119, i32 117, i32 268, i32 186, i32 269, i32 11, i32 2, i32 124,
	i32 115, i32 142, i32 41, i32 87, i32 318, i32 238, i32 173, i32 27,
	i32 148, i32 203, i32 359, i32 204, i32 315, i32 237, i32 1, i32 195,
	i32 239, i32 328, i32 44, i32 253, i32 149, i32 18, i32 317, i32 86,
	i32 347, i32 320, i32 41, i32 272, i32 247, i32 277, i32 94, i32 209,
	i32 28, i32 41, i32 78, i32 262, i32 250, i32 144, i32 108, i32 248,
	i32 11, i32 105, i32 137, i32 16, i32 122, i32 66, i32 157, i32 22,
	i32 349, i32 341, i32 102, i32 204, i32 340, i32 63, i32 58, i32 217,
	i32 348, i32 110, i32 173, i32 305, i32 317, i32 381, i32 338, i32 9,
	i32 311, i32 120, i32 98, i32 105, i32 182, i32 280, i32 215, i32 111,
	i32 240, i32 49, i32 20, i32 279, i32 257, i32 72, i32 252, i32 155,
	i32 39, i32 347, i32 35, i32 336, i32 38, i32 353, i32 223, i32 304,
	i32 108, i32 362, i32 21, i32 343, i32 183, i32 333, i32 278, i32 220,
	i32 15, i32 214, i32 79, i32 79, i32 257, i32 214, i32 284, i32 291,
	i32 152, i32 21, i32 218, i32 346, i32 50, i32 51, i32 327, i32 372,
	i32 225, i32 362, i32 94, i32 233, i32 207, i32 358, i32 16, i32 256,
	i32 123, i32 355, i32 160, i32 45, i32 315, i32 194, i32 116, i32 63,
	i32 166, i32 206, i32 201, i32 14, i32 290, i32 111, i32 240, i32 175,
	i32 60, i32 337, i32 312, i32 332, i32 121, i32 361, i32 2, i32 371,
	i32 306, i32 265, i32 278, i32 226, i32 208, i32 336, i32 334, i32 278,
	i32 6, i32 247, i32 351, i32 261, i32 185, i32 17, i32 369, i32 348,
	i32 77, i32 251, i32 312, i32 321, i32 131, i32 333, i32 361, i32 83,
	i32 211, i32 12, i32 34, i32 119, i32 341, i32 273, i32 328, i32 263,
	i32 85, i32 232, i32 323, i32 18, i32 301, i32 202, i32 271, i32 72,
	i32 379, i32 95, i32 165, i32 267, i32 189, i32 82, i32 377, i32 241,
	i32 246, i32 337, i32 154, i32 36, i32 151, i32 373, i32 376, i32 144,
	i32 56, i32 113, i32 247, i32 298, i32 326, i32 297, i32 37, i32 225,
	i32 377, i32 201, i32 115, i32 239, i32 14, i32 233, i32 329, i32 310,
	i32 146, i32 43, i32 197, i32 219, i32 237, i32 98, i32 340, i32 168,
	i32 16, i32 48, i32 107, i32 97, i32 188, i32 282, i32 27, i32 128,
	i32 29, i32 353, i32 310, i32 175, i32 291, i32 199, i32 128, i32 44,
	i32 257, i32 262, i32 149, i32 8, i32 196, i32 226, i32 222, i32 283,
	i32 354, i32 367, i32 323, i32 366, i32 132, i32 365, i32 42, i32 341,
	i32 326, i32 33, i32 382, i32 46, i32 143, i32 279, i32 217, i32 270,
	i32 258, i32 138, i32 62, i32 132, i32 345, i32 48, i32 160, i32 307,
	i32 244, i32 270, i32 233, i32 268, i32 361, i32 297, i32 46, i32 164,
	i32 267, i32 350, i32 264, i32 186, i32 357, i32 220, i32 320, i32 18,
	i32 8, i32 0, i32 194, i32 255, i32 124, i32 59, i32 141, i32 177,
	i32 285, i32 360, i32 274, i32 225, i32 313, i32 303, i32 150, i32 142,
	i32 339, i32 336, i32 126, i32 338, i32 160, i32 162, i32 259, i32 236,
	i32 202, i32 363, i32 26, i32 283, i32 271, i32 332, i32 82, i32 303,
	i32 127, i32 314, i32 101, i32 148, i32 311, i32 176, i32 286, i32 54,
	i32 162, i32 167, i32 131, i32 37, i32 299, i32 360, i32 206, i32 177,
	i32 22, i32 112, i32 90, i32 50, i32 60, i32 122, i32 83, i32 127,
	i32 229, i32 163, i32 313, i32 166, i32 290, i32 292, i32 260, i32 232,
	i32 275, i32 4, i32 269, i32 356, i32 170, i32 2, i32 320, i32 280,
	i32 116, i32 381, i32 238, i32 179, i32 19, i32 210, i32 89, i32 65,
	i32 30, i32 205, i32 349, i32 188, i32 252, i32 59, i32 111, i32 271,
	i32 32, i32 192, i32 128, i32 159, i32 367, i32 250, i32 140, i32 363,
	i32 153, i32 17, i32 249, i32 235, i32 75, i32 74, i32 15, i32 169,
	i32 85, i32 337, i32 124, i32 270, i32 281, i32 251, i32 370, i32 277,
	i32 34, i32 187, i32 118, i32 139, i32 122, i32 106, i32 347, i32 185,
	i32 379, i32 299, i32 180, i32 246, i32 310, i32 190, i32 354, i32 344,
	i32 54, i32 47, i32 28, i32 145, i32 210, i32 147, i32 35, i32 370,
	i32 173, i32 199, i32 208, i32 230, i32 304, i32 75, i32 161, i32 196,
	i32 1, i32 216, i32 293, i32 366, i32 359, i32 159, i32 12, i32 155,
	i32 151, i32 181, i32 76, i32 103, i32 112, i32 325, i32 243, i32 65,
	i32 66, i32 302, i32 45, i32 245, i32 226, i32 109, i32 7, i32 242,
	i32 55, i32 238, i32 64, i32 344, i32 255, i32 20, i32 109, i32 101,
	i32 62, i32 195, i32 142, i32 236, i32 330, i32 7, i32 186, i32 359,
	i32 170, i32 50, i32 302, i32 115, i32 141, i32 194, i32 166, i32 80,
	i32 113, i32 281, i32 17, i32 73, i32 284, i32 89, i32 234, i32 87,
	i32 120, i32 184, i32 296, i32 331, i32 309, i32 221, i32 240, i32 221,
	i32 187, i32 0, i32 135, i32 153, i32 106, i32 11, i32 90, i32 31,
	i32 175, i32 372, i32 136, i32 364, i32 316, i32 367, i32 294, i32 231,
	i32 235, i32 40, i32 382, i32 293, i32 200, i32 139, i32 340, i32 342,
	i32 25, i32 376, i32 73, i32 267, i32 295, i32 381, i32 27, i32 67,
	i32 88, i32 322, i32 174, i32 95, i32 113, i32 31, i32 104, i32 269,
	i32 37, i32 207, i32 72, i32 330, i32 319, i32 108, i32 123, i32 242,
	i32 87, i32 209, i32 86, i32 358, i32 93, i32 205, i32 129, i32 281,
	i32 178, i32 296, i32 211, i32 191, i32 378, i32 290, i32 255, i32 295,
	i32 252, i32 327, i32 316, i32 313, i32 202, i32 163, i32 130, i32 210,
	i32 227, i32 300, i32 228, i32 287, i32 174, i32 204, i32 10, i32 49,
	i32 324, i32 374, i32 91, i32 374, i32 150, i32 62, i32 136, i32 150,
	i32 61, i32 183, i32 209, i32 117, i32 137, i32 334, i32 84, i32 376,
	i32 159, i32 297, i32 143, i32 355, i32 264, i32 82, i32 70, i32 241,
	i32 136, i32 253, i32 234, i32 125, i32 378, i32 54, i32 110, i32 130,
	i32 88, i32 176, i32 23, i32 74, i32 129, i32 31, i32 73, i32 276,
	i32 357, i32 158, i32 23, i32 4, i32 224, i32 170, i32 365, i32 123,
	i32 256, i32 356, i32 190, i32 351, i32 114, i32 172, i32 32, i32 3,
	i32 164, i32 179, i32 193, i32 229, i32 298, i32 30, i32 19, i32 213,
	i32 275, i32 93, i32 36, i32 5, i32 317, i32 325, i32 304, i32 244,
	i32 197, i32 338, i32 155, i32 294, i32 315, i32 230, i32 251, i32 306,
	i32 224, i32 187, i32 300, i32 380, i32 76, i32 63, i32 285, i32 216,
	i32 147, i32 248, i32 121, i32 134, i32 319, i32 219, i32 100, i32 39,
	i32 236, i32 228, i32 350, i32 68, i32 26, i32 75, i32 78, i32 274,
	i32 217, i32 24, i32 152, i32 38, i32 363, i32 244, i32 133, i32 103,
	i32 311, i32 57, i32 165, i32 91, i32 61, i32 132, i32 325, i32 46,
	i32 133, i32 259, i32 145, i32 78, i32 253, i32 275, i32 213, i32 154,
	i32 227, i32 348, i32 83, i32 375, i32 222, i32 373, i32 61, i32 96,
	i32 227, i32 288, i32 178, i32 153, i32 354, i32 118, i32 212, i32 193,
	i32 6, i32 15, i32 74, i32 344, i32 146, i32 52, i32 70, i32 23,
	i32 158, i32 126, i32 65, i32 112, i32 284, i32 273, i32 55, i32 53,
	i32 260, i32 309, i32 107, i32 135, i32 265, i32 274, i32 80, i32 268,
	i32 372, i32 272, i32 129, i32 64, i32 152
], align 4

@marshal_methods_number_of_classes = dso_local local_unnamed_addr constant i32 0, align 4

@marshal_methods_class_cache = dso_local local_unnamed_addr global [0 x %struct.MarshalMethodsManagedClass] zeroinitializer, align 8

; Names of classes in which marshal methods reside
@mm_class_names = dso_local local_unnamed_addr constant [0 x ptr] zeroinitializer, align 8

@mm_method_names = dso_local local_unnamed_addr constant [1 x %struct.MarshalMethodName] [
	%struct.MarshalMethodName {
		i64 u0x0000000000000000, ; name: 
		ptr @.MarshalMethodName.0_name; char* name
	} ; 0
], align 8

; get_function_pointer (uint32_t mono_image_index, uint32_t class_index, uint32_t method_token, void*& target_ptr)
@get_function_pointer = internal dso_local unnamed_addr global ptr null, align 8

; Functions

; Function attributes: memory(write, argmem: none, inaccessiblemem: none) "min-legal-vector-width"="0" mustprogress "no-trapping-math"="true" nofree norecurse nosync nounwind "stack-protector-buffer-size"="8" uwtable willreturn
define void @xamarin_app_init(ptr nocapture noundef readnone %env, ptr noundef %fn) local_unnamed_addr #0
{
	%fnIsNull = icmp eq ptr %fn, null
	br i1 %fnIsNull, label %1, label %2

1: ; preds = %0
	%putsResult = call noundef i32 @puts(ptr @.str.0)
	call void @abort()
	unreachable 

2: ; preds = %1, %0
	store ptr %fn, ptr @get_function_pointer, align 8, !tbaa !3
	ret void
}

; Strings
@.str.0 = private unnamed_addr constant [40 x i8] c"get_function_pointer MUST be specified\0A\00", align 1

;MarshalMethodName
@.MarshalMethodName.0_name = private unnamed_addr constant [1 x i8] c"\00", align 1

; External functions

; Function attributes: "no-trapping-math"="true" noreturn nounwind "stack-protector-buffer-size"="8"
declare void @abort() local_unnamed_addr #2

; Function attributes: nofree nounwind
declare noundef i32 @puts(ptr noundef) local_unnamed_addr #1
attributes #0 = { memory(write, argmem: none, inaccessiblemem: none) "min-legal-vector-width"="0" mustprogress "no-trapping-math"="true" nofree norecurse nosync nounwind "stack-protector-buffer-size"="8" "target-cpu"="generic" "target-features"="+fix-cortex-a53-835769,+neon,+outline-atomics,+v8a" uwtable willreturn }
attributes #1 = { nofree nounwind }
attributes #2 = { "no-trapping-math"="true" noreturn nounwind "stack-protector-buffer-size"="8" "target-cpu"="generic" "target-features"="+fix-cortex-a53-835769,+neon,+outline-atomics,+v8a" }

; Metadata
!llvm.module.flags = !{!0, !1, !7, !8, !9, !10}
!0 = !{i32 1, !"wchar_size", i32 4}
!1 = !{i32 7, !"PIC Level", i32 2}
!llvm.ident = !{!2}
!2 = !{!".NET for Android remotes/origin/release/9.0.1xx @ be1cab92326783479054e72990da08008e5be819"}
!3 = !{!4, !4, i64 0}
!4 = !{!"any pointer", !5, i64 0}
!5 = !{!"omnipotent char", !6, i64 0}
!6 = !{!"Simple C++ TBAA"}
!7 = !{i32 1, !"branch-target-enforcement", i32 0}
!8 = !{i32 1, !"sign-return-address", i32 0}
!9 = !{i32 1, !"sign-return-address-all", i32 0}
!10 = !{i32 1, !"sign-return-address-with-bkey", i32 0}
