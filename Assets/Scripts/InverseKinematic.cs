using UnityEditor;
using UnityEngine;

public class InverseKinematic : MonoBehaviour
{
    [SerializeField] private int _chainLength = 2;
    [SerializeField] private int _iterations = 10;
    [SerializeField] private float _delta = 0.001f;
    [SerializeField] private Transform _target;
    
    private Transform _pole;
    
    private Transform[] _vertices;
    private Vector3[] _positions;
    private float[] _verticesDistances;
    
    private float _totalLength;

    private void Init() {
        _vertices = new Transform[_chainLength + 1];
        _positions = new Vector3[_chainLength + 1];
        _verticesDistances = new float[_chainLength];
        
        _totalLength = 0;
        
        // init data
        Transform current = this.transform;
        for (int i = _vertices.Length - 1; i >= 0; i--) {
            _vertices[i] = current;
            _positions[i] = current.position;
            
            if (i < _vertices.Length - 1) {
                _verticesDistances[i] = Vector3.Distance(_vertices[i].position, _vertices[i + 1].position);
                _totalLength += _verticesDistances[i];
            }
            
            if(current.parent.transform.tag.Equals("Joints")) current = current.parent;
        }
    }

    private void Awake() {
        // Pole is the first child of the target game object
        _pole = _target.GetChild(0);
        Init();
    }
    
    private void LateUpdate() {
        InverseKinematicAlgorithm();
    }

    // Inverse Kinematic Algorithm (FK -> IK)
    private void InverseKinematicAlgorithm() {
        if(_target == null) {
            return;
        }
        
        if(_verticesDistances.Length != _chainLength) {
            Init();
        }
        
        // Get positions
        for(int i = 0; i<_vertices.Length; i++) {
            _positions[i] = _vertices[i].position;
        }
        
        //Calculate distance
        if((_target.position - _vertices[0].position).sqrMagnitude >= _totalLength * _totalLength) {
            // Stretch
            Vector3 direction = (_target.position - _positions[0]).normalized;
            // Move the positions behind the root (this)
            for(int i = 1; i < _positions.Length; i++) {
                _positions[i] = _positions[i - 1] + direction * _verticesDistances[i - 1];
            }
        } else {
            for(int iteration = 0; iteration < _iterations; iteration++) {
                // Back
                for(int i = _positions.Length - 1; i > 0; i--) {
                    if(i == _positions.Length - 1) {
                        _positions[i] = _target.position;
                    } else {
                        _positions[i] = _positions[i+1] + (_positions[i] - _positions[i+1]).normalized * _verticesDistances[i];
                    }
                }
                
                // Forward
                for(int i = 1; i < _positions.Length; i++) {
                    _positions[i] = _positions[i-1] + (_positions[i] - _positions[i-1]).normalized * _verticesDistances[i-1];
                }
                
                // Check distance
                if((_positions[_positions.Length - 1] - _target.position).sqrMagnitude < _delta * _delta) {
                    break;
                }
            }
        }
        
        // Move towards pole
        if(_pole != null) {
            for(int i = 1; i < _positions.Length - 1; i++) {
                Plane plane = new Plane(_positions[i+1] - _positions[i-1], _positions[i-1]);
                Vector3 projectedPole = plane.ClosestPointOnPlane(_pole.position);
                Vector3 projectedBone = plane.ClosestPointOnPlane(_positions[i]);
                
                float angle = Vector3.SignedAngle(projectedBone - _positions[i-1], projectedPole - _positions[i-1], plane.normal);
                _positions[i] = Quaternion.AngleAxis(angle, plane.normal) * (_positions[i] - _positions[i-1]) + _positions[i-1];
            }
        }
        
        // Set positions & rotations
        for(int i = 0; i < _vertices.Length; i++) {
            if(i == _vertices.Length - 1) {
                _vertices[i].rotation = Quaternion.FromToRotation(_vertices[i].position - _vertices[i-1].position, _target.position - _vertices[i-1].position) * _vertices[i].rotation;
            } else {
                _vertices[i].rotation = Quaternion.FromToRotation(_vertices[i].position - _vertices[i+1].position, _positions[i] - _vertices[i+1].position) * _vertices[i].rotation;
            }
            _vertices[i].position = _positions[i];
        }
        
    }

    private void OnDrawGizmos() {
        Transform current = this.transform;
        int cpt = 0;
        while(cpt < _chainLength && current != null && current.parent.transform.tag.Equals("Joints")) {
            float scale = Vector3.Distance(current.position, current.parent.position) * 0.1f;
            Handles.matrix = Matrix4x4.TRS(current.position, Quaternion.FromToRotation(Vector3.up, current.parent.position - current.position), new Vector3(scale, Vector3.Distance(current.position, current.parent.position), scale));
            Handles.color = Color.green;
            Handles.DrawWireCube(Vector3.up * 0.5f, Vector3.one);
            current = current.parent;
        }
    }
}
