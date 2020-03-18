using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Phone_Forecast.Models.DbContexts;
using System.Linq;
using System.Threading.Tasks;

namespace Phone_Forecast.Controllers
{
    public class HardwaresController : Controller
    {
        private readonly HardwareContext _context;

        public HardwaresController(HardwareContext context)
        {
            _context = context;
        }

        // GET: Hardwares
        public async Task<IActionResult> Index()
        {
            return View(await _context.HardwareConfigurations.ToListAsync());
        }

        // GET: Hardwares/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hardware = await _context.HardwareConfigurations
                .FirstOrDefaultAsync(m => m.ConfigId == id);
            if (hardware == null)
            {
                return NotFound();
            }

            return View(hardware);
        }

        // GET: Hardwares/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Hardwares/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ConfigId,PhoneModel,InternalStorageSpace,HasMemoryCardReader,Cpu,ProcessorCoreCount,ProcessorCoreSpeed,RAM,HasGPU,GPU,HasHeadphoneOutput,Has2g,Has3g,Has4g,Has5g,HasBluetooth,HasGps,HasWifi,HasRearCamera,HasFrontCamera,FrontCameraMegapixel,RearCameraMegapixel,MaximumLensAperture,RearCameraCount,CanRecordVideo,MaxFramerateMaxResolution,MaxFramerateMinResolution,BatteryCapacity,HasExchangableBattery,Depth,Height,Width,Weight,HasWirelessCharging,WirelessStandard,HasDualSim,SimCard,HasFastCharging,IsWaterResistant,OriginalPrice,ReleaseDate,ProductPage,IsSelected")] Hardware hardware)
        {
            if (ModelState.IsValid)
            {
                _context.Add(hardware);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(hardware);
        }

        // GET: Hardwares/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hardware = await _context.HardwareConfigurations.FindAsync(id);
            if (hardware == null)
            {
                return NotFound();
            }
            return View(hardware);
        }

        // POST: Hardwares/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ConfigId,PhoneModel,InternalStorageSpace,HasMemoryCardReader,Cpu,ProcessorCoreCount,ProcessorCoreSpeed,RAM,HasGPU,GPU,HasHeadphoneOutput,Has2g,Has3g,Has4g,Has5g,HasBluetooth,HasGps,HasWifi,HasRearCamera,HasFrontCamera,FrontCameraMegapixel,RearCameraMegapixel,MaximumLensAperture,RearCameraCount,CanRecordVideo,MaxFramerateMaxResolution,MaxFramerateMinResolution,BatteryCapacity,HasExchangableBattery,Depth,Height,Width,Weight,HasWirelessCharging,WirelessStandard,HasDualSim,SimCard,HasFastCharging,IsWaterResistant,OriginalPrice,ReleaseDate,ProductPage,IsSelected")] Hardware hardware)
        {
            if (id != hardware.ConfigId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(hardware);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HardwareExists(hardware.ConfigId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(hardware);
        }

        // GET: Hardwares/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hardware = await _context.HardwareConfigurations
                .FirstOrDefaultAsync(m => m.ConfigId == id);
            if (hardware == null)
            {
                return NotFound();
            }

            return View(hardware);
        }

        // POST: Hardwares/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var hardware = await _context.HardwareConfigurations.FindAsync(id);
            _context.HardwareConfigurations.Remove(hardware);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HardwareExists(int id)
        {
            return _context.HardwareConfigurations.Any(e => e.ConfigId == id);
        }
    }
}
