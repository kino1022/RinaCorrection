using RinaCorrection.Definition;
using VContainer;
using VContainer.Unity;

namespace RinaCorrection.Installer {
    public class RinaCorrection_ManagerInstaller : IInstaller {
        public void Install(IContainerBuilder builder) {

            builder
                .Register<CorrectionManager>(Lifetime.Transient)
                .AsImplementedInterfaces();

            builder
                .Register<FixedType>(Lifetime.Transient);

        }
    }
}