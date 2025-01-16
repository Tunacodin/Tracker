using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.HelperClass;

namespace Application.Utilities.Constants
{
    public static class Device
    {
        public static MobileDevice GetDevice()
        {
            var random = new Random();
            int index = random.Next(devices.Count);
            return devices[index];
        }

        private static readonly List<MobileDevice> devices = new List<MobileDevice>
        {
            new MobileDevice { DeviceId = "R16NW", DeviceModel = "SM-G930F" }, // Samsung Galaxy S7
            new MobileDevice { DeviceId = "B12QW", DeviceModel = "Pixel5" }, // iPhone 11
            new MobileDevice { DeviceId = "X45RT", DeviceModel = "Pixel5" }, // Google Pixel 5
            new MobileDevice { DeviceId = "Z78YU", DeviceModel = "Redmi Note 9" }, // Xiaomi Redmi Note 9
            new MobileDevice { DeviceId = "G89HJ", DeviceModel = "P30Pro" }, // Huawei P30 Pro
            new MobileDevice { DeviceId = "C34PK", DeviceModel = "Galaxy S21" }, // Samsung Galaxy S21
            new MobileDevice { DeviceId = "H67VB", DeviceModel = "OnePlus 9" }, // OnePlus 9
            new MobileDevice { DeviceId = "M12XZ", DeviceModel = "Pixel5" }, // iPhone 12 Pro
            new MobileDevice { DeviceId = "K78PL", DeviceModel = "Xperia Z5" },
        };
    }
}
