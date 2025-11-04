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
                .RegisterInstance(new FixedType());

            builder
                .RegisterInstance(new RatioType());

        }
    }
}