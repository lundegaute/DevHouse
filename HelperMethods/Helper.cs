
namespace DevHouse.Helper {
    public static class ValidationHelper {
        public static void CheckIfExistsOrException(params (object entity, string typeName)[] entities ){
            foreach(var (entity, name) in entities){
                if ( entity is null) {
                    throw new KeyNotFoundException($"{name} Not Found");
                }
            }
        }
        public static void IdInRangeOrException(int id, int minimumId = 1){
            if ( id < minimumId) {
                throw new IndexOutOfRangeException($"Id must be greater than: {minimumId}");
            }
        }
        public static void CheckIfNotInDatabaseOrException(bool entity, string typeName) {
            if ( entity ) {
                throw new ArgumentException($"{typeName} already in database");
            }
        }
        public static void CheckIfIdMatchBodyIdOrException(int id, int bodyId, string typeName) {
            if ( id != bodyId) {
                throw new ArgumentException($"URL Id not matching {typeName} Id");
            }
        }
    }
}