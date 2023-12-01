using UnityEngine;
using System.Collections;

namespace Pathfinding {
	/// <summary>
	/// Sets the destination of an AI to the position of a specified object.
	/// This component should be attached to a GameObject together with a movement script such as AIPath, RichAI or AILerp.
	/// This component will then make the AI move towards the <see cref="Target"/> set on this component.
	///
	/// See: <see cref="Pathfinding.IAstarAI.destination"/>
	///
	/// [Open online documentation to see images]
	/// </summary>
	[UniqueComponent(tag = "ai.destination")]
	[HelpURL("http://arongranberg.com/astar/docs/class_pathfinding_1_1_a_i_destination_setter.php")]
	public class AIDestinationSetter : VersionedMonoBehaviour {
        /// <summary>The object that the AI should move to</summary>
		/// 
        [Header("Movement")]
        [SerializeField] private float _agrDictance;
		[SerializeField] private Transform _leftPatrolPosition;
		[SerializeField] private Transform _rightPatrolPosition;
		public Transform CurrentTarget;

        public Transform Target;
		IAstarAI ai;

		void OnEnable () {
			ai = GetComponent<IAstarAI>();
			// Update the destination right before searching for a path as well.
			// This is enough in theory, but this script will also update the destination every
			// frame as the destination is used for debugging and may be used for other things by other
			// scripts as well. So it makes sense that it is up to date every frame.
			if (ai != null) ai.onSearchPath += Update;
		}

		void OnDisable () {
			if (ai != null) ai.onSearchPath -= Update;
		}

        private void Start()
        {
			CurrentTarget = _rightPatrolPosition;
        }

        /// <summary>Updates the AI's destination every frame</summary>
        void Update () {
			if (Target != null && ai != null)
			{
                ai.destination = CurrentTarget.position;

                if (Vector2.Distance(transform.position, Target.position) > _agrDictance)
                    StartCoroutine(Patrol());

                if (Vector2.Distance(transform.position, Target.position) < _agrDictance)	
					CurrentTarget = Target;
			}
		}

        private IEnumerator Patrol()
        {

			if (Vector2.Distance(transform.position, CurrentTarget.position) < 1)
			{
				if (CurrentTarget == _rightPatrolPosition)
				{
					yield return new WaitForSeconds(2);
                    CurrentTarget = _leftPatrolPosition;
				}
				else if (CurrentTarget == _leftPatrolPosition)
				{
					yield return new WaitForSeconds(2);
                    CurrentTarget = _rightPatrolPosition;
				}
			}
        }
    }
}