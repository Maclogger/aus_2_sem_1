namespace Entities
{
    public class Config
    {
        private static readonly Config _instance = new Config();

        // double TOLERANCE
        private double _doubleTolerance = 0.00001;
        // random generated coordination:
        private double _minLatitude = Double.MinValue;
        private double _maxLatitude = Double.MaxValue;
        private double _minLongitude = Double.MinValue;
        private double _maxLongitude = Double.MinValue;

        // testing KdTree structure:
        private double _probOfAdd = 0.25;
        private double _probOfFind = 0.25;
        private double _probOfRemove = 0.25;
        private double _probOfUpdate = 0.25;

        // printing format
        private bool _formattedOutput = false;

        private Config() { }

        public static Config Instance => _instance;

        public bool ProbabilityCheck()
        {
            return Math.Abs(_probOfAdd + _probOfFind + _probOfRemove + _probOfUpdate - 1) < DoubleTolerance;
        }

        public double MinLatitude
        {
            get => _minLatitude;
            set => _minLatitude = value;
        }

        public double MaxLatitude
        {
            get => _maxLatitude;
            set => _maxLatitude = value;
        }

        public double MinLongitude
        {
            get => _minLongitude;
            set => _minLongitude = value;
        }

        public double MaxLongitude
        {
            get => _maxLongitude;
            set => _maxLongitude = value;
        }

        public double ProbOfAdd
        {
            get => _probOfAdd;
            set => _probOfAdd = value;
        }

        public double ProbOfFind
        {
            get => _probOfFind;
            set => _probOfFind = value;
        }

        public double ProbOfRemove
        {
            get => _probOfRemove;
            set => _probOfRemove = value;
        }

        public double ProbOfUpdate
        {
            get => _probOfUpdate;
            set => _probOfUpdate = value;
        }

        public bool FormattedOutput
        {
            get => _formattedOutput;
            set => _formattedOutput = value;
        }

        public double DoubleTolerance
        {
            get => _doubleTolerance;
            set => _doubleTolerance = value;
        }
    }
}