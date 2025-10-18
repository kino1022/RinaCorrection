using RinaCorrection.Modules;
using VContainer;
using VContainer.Unity;

namespace RinaCorrection.Installer {
    public class CorrectionManagerInstaller : IInstaller {
        public void Install(IContainerBuilder builder) {
            
            builder
                .RegisterFactory<ICorrectionManager, ICorrectionChangeHandler>(manager => {
                    return new CorrectionChangeHandler(manager);
                });

            builder
                .Register<ICorrectionManager, CorrectionManager>(Lifetime.Transient)
                .As<ICorrector>();
        }
    }
}