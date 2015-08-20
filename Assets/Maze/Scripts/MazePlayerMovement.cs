using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class MazePlayerMovement : MonoBehaviour
{
    private HFTInput m_hftInput;
    private Rigidbody2D _rigidbody;

    public float moveDistance = 1f;

    protected bool prevHMove = false;
    protected bool prevVMove = false;

    protected GridManager grid
    {
        get
        {
            if (_grid == null)
            {
                _grid = GridManager.Instance;
            }
            return _grid;
        }
    }
    private GridManager _grid;

    public Vector3 gridOffset = new Vector3(0, -0.2f, 0);

    protected PlayerScore score
    {
        get
        {
            return playerUI.score;
        }
    }

    protected MazePlayerUI playerUI;

    public Action onBlockHit = delegate () { };
    public Action<Collider2D> onRewardHit = delegate (Collider2D col) { };

    protected void Start()
    {
        m_hftInput = GetComponent<HFTInput>();
        _rigidbody = GetComponent<Rigidbody2D>();
        grid.SnapToGrid(transform);
        transform.position += gridOffset;

        playerUI = GetComponent<MazePlayerUI>();
        score.catchPlayerEvent += MoveToRandomSpawnPoint;
        onRewardHit += score.CollideWithScore;
    }

    void MoveToRandomSpawnPoint()
    {
        // Pick a random spawn point
        int ndx = UnityEngine.Random.Range(0, LevelSettings.settings.spawnPoints.Length - 1);
        transform.localPosition = LevelSettings.settings.spawnPoints[ndx].localPosition;

        grid.SnapToGrid(transform);
        transform.position += gridOffset;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Get left/right input (get both phone and local input)
        float hMove = m_hftInput.GetAxis("Horizontal") + Input.GetAxis("Horizontal");
        float vMove = m_hftInput.GetAxis("Vertical") + Input.GetAxis("Vertical");

        float minMove = 0.05f;
        GridVector ijPos = grid.xyToij(transform.position-gridOffset);

        bool justMoved = false;

        if (Mathf.Abs(hMove) > minMove && Mathf.Abs(hMove) >= Mathf.Abs(vMove) && !prevHMove)
        {
            justMoved = true;
            if (hMove > 0)
            {
                ijPos.x += 1;
            }
            else
            {
                ijPos.x -= 1;
            }

            //transform.position += Vector3.right * move * moveDistance;
            prevHMove = true;
        }
        if (Mathf.Abs(hMove) < minMove)
        {
            prevHMove = false;
        }

        if (Mathf.Abs(vMove) > minMove && Mathf.Abs(vMove) > Mathf.Abs(hMove) && !prevVMove)
        {
            justMoved = true;
            if (vMove < 0)
            {
                ijPos.y += 1;
            }
            else
            {
                ijPos.y -= 1;
            }
            prevVMove = true;
        }
        if (Mathf.Abs(vMove) < minMove)
        {
            prevVMove = false;
        }

        if(justMoved)
        {
            Vector3 toMove = grid.ijToxyz(ijPos);
            string obstacleTag = "Obstacle";
            string playerTag = "Player";
            string rewardTag = "Reward";
            IEnumerable<Collider2D> cols;
            if (grid.IsTagAtPos(toMove, playerTag, out cols))
            {
                //Hit other player maybe
                foreach(var collider in cols)
                {
                    MazePlayerUI otherPlayer = collider.GetComponent<MazePlayerUI>();
                    if (otherPlayer != null)
                    {
                        otherPlayer.score.CollideWithPlayer(score);
                    }
                }
            }
            else if(grid.IsTagAtPos(toMove,obstacleTag))
            {
                onBlockHit();
            }
            else
            {
                if (grid.IsTagAtPos(toMove, rewardTag, out cols))
                {
                    onRewardHit(cols.FirstOrDefault());
                }
                //actually move!
                transform.position = toMove;
                transform.position += gridOffset;
            }
        }
    }
}
