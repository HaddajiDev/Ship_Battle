using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System.Linq;

public class Player : MonoBehaviour
{
    private Camera mainCamera;
    private Vector2 startDragPos;
    private LineRenderer trajectoryLineRenderer;
    private float maxPullDistance;
    public float maxForce = 50f;
    public GameObject BulletPrefab;

    [Header("Line Renderer")]
    public GameObject trajectoryLinePrefab;
    public int linePoints = 20;

    [Header("Others")]
    public Transform shootPoint;
    public GameObject cannonFireEffect;
    public Bullets_Data bulletsData;
    private int damage;

    public bool ready = true;
    public int burstCount;
    public bool inFire = false;

    private Animator anim;
    private CinemachineImpulseSource impulseSource;
    [HideInInspector] public int _selectedBullet;
    public List<(int, int)> _bulletsLimit = new List<(int, int)>();

    void Start()
    {
        ready = false;
        maxPullDistance = maxForce;
        mainCamera = Camera.main;
        InitializeTrajectoryLineRenderer();
        anim = GetComponent<Animator>();
        impulseSource = GetComponent<CinemachineImpulseSource>();
        SelectBullet(0);
    }

    void Update()
    {
        if (ready)
        {
            if (Input.GetMouseButtonDown(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began))
            {
                if (Input.touchCount > 0)
                {
                    startDragPos = mainCamera.ScreenToWorldPoint(Input.GetTouch(0).position);
                }
                else
                {
                    startDragPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
                }

                trajectoryLineRenderer.enabled = true;
            }

            if (Input.GetMouseButton(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved))
            {
                Vector2 currentMousePos;
                if (Input.touchCount > 0)
                {
                    currentMousePos = mainCamera.ScreenToWorldPoint(Input.GetTouch(0).position);
                }
                else
                {
                    currentMousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
                }

                DrawTrajectory(startDragPos, currentMousePos);
            }

            if (Input.GetMouseButtonUp(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended))
            {
                trajectoryLineRenderer.enabled = false;
                Vector2 endDragPos;
                if (Input.touchCount > 0)
                {
                    endDragPos = mainCamera.ScreenToWorldPoint(Input.GetTouch(0).position);
                }
                else
                {
                    endDragPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
                }

                Vector2 swipeDirection = endDragPos - startDragPos;
                float swipeMagnitude = swipeDirection.magnitude;

                swipeMagnitude = Mathf.Min(swipeMagnitude, maxPullDistance);
                float forceStrength = swipeMagnitude / maxPullDistance * maxForce;

                if (GameManager.Instance.Fire_Uses == 0)
                    inFire = false;

                if (getLimit() == 0)
                    SelectBullet(0);

                if (burstCount > 1 && GameManager.Instance.Burst_Uses != 0)
                {
                    for (int i = 0; i < burstCount; i++)
                    {
                        shootBurst(swipeDirection, forceStrength);
                    }
                    GameManager.Instance.Burst_Uses--;
                }
                else
                {
                    shootOne(swipeDirection, forceStrength);
                }

                setLimit();
                afterShoot();                
            }
        }
    }

    void shootOne(Vector2 swipeDirection, float forceStrength)
    {
        GameObject bullet = Instantiate(BulletPrefab, shootPoint.position, Quaternion.identity);
        bullet.GetComponent<Bullet>().Player_Bullet = true;
        bullet.GetComponent<Bullet>().Damage = damage;
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.AddForce(-swipeDirection.normalized * forceStrength, ForceMode2D.Impulse);
        if (inFire)
        {
            bullet.GetComponent<Bullet>().inFire = true;
        }
    }

    void shootBurst(Vector2 swipeDirection, float forceStrength)
    {
        Vector3 spreadVector = new Vector3(Random.Range(-2, 2), Random.Range(-2, 2));
        GameObject bullet = Instantiate(BulletPrefab, shootPoint.position, Quaternion.identity);
        bullet.GetComponent<Bullet>().Player_Bullet = true;
        bullet.GetComponent<Bullet>().Damage = damage;
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.AddForce(-swipeDirection.normalized * forceStrength, ForceMode2D.Impulse);
        rb.velocity = (shootPoint.right * forceStrength) + spreadVector;
        if (inFire)
        {
            bullet.GetComponent<Bullet>().inFire = true;
        }
    }

    void afterShoot()
    {
        if (inFire && GameManager.Instance.Fire_Uses != 0)
            GameManager.Instance.Fire_Uses--;

        if (cannonFireEffect != null)
            Instantiate(cannonFireEffect, shootPoint.position, transform.rotation);

        this.enabled = false;
        ready = false;
        GetComponent<Rotate_Object>().enabled = false;
        GameManager.Instance.isChecking = false;
        anim.SetTrigger("shoot");
        Camera_Shake.Instance.Shake(impulseSource, 1);
    }

    private void InitializeTrajectoryLineRenderer()
    {
        GameObject trajectoryLineObject = Instantiate(trajectoryLinePrefab, shootPoint.position, Quaternion.identity);
        trajectoryLineRenderer = trajectoryLineObject.GetComponent<LineRenderer>();
        trajectoryLineRenderer.enabled = false;
        trajectoryLineRenderer.startColor = Color.black;
        trajectoryLineRenderer.endColor = Color.black;
    }


    private Vector3[] CalculateTrajectoryPoints(Vector2 start, Vector2 startVelocity, Vector2 gravity)
    {
        float timestep = 0.05f;
        Vector3[] points = new Vector3[linePoints];
        for (int i = 0; i < linePoints; i++)
        {
            float t = i * timestep;
            points[i] = start + startVelocity * t + 0.5f * gravity * t * t;
        }
        return points;
    }

    private void DrawTrajectory(Vector2 startPos, Vector2 endPos)
    {
        Vector2 forceDirection = (endPos - startPos).normalized;
        float forceMagnitude = (endPos - startPos).magnitude;

        Vector3[] trajectoryPoints = CalculateTrajectoryPoints(startPos, -forceDirection * forceMagnitude, Physics2D.gravity);
        Vector2 offset = (Vector2)transform.position - startPos;
        for (int i = 0; i < trajectoryPoints.Length; i++)
        {
            trajectoryPoints[i] += (Vector3)offset;
        }
        trajectoryLineRenderer.positionCount = trajectoryPoints.Length;
        trajectoryLineRenderer.SetPositions(trajectoryPoints);
    }

    private void ReadyDelay()
    {
        ready = true;
    }

    public void ReadyUp()
    {
        Invoke("ReadyDelay", 0.2f);
        GameManager.Instance.Get_Ready_UI(0, 0.3f);
    }

    private void GetBullet(int index)
    {
        Bullets bullet = bulletsData.Get_Bullet(index);
        BulletPrefab = bullet.Bullet_Prefab;
        damage = bullet.Damage;
        if(index != 0)
        {
            if (!_bulletsLimit.Any(limit => limit.Item1 == index))
            {
                _bulletsLimit.Add((index, bullet.Limit));
            }
        }        
    }

    public void SelectBullet(int index)
    {
        GetBullet(index);
        _selectedBullet = index;        
    }

    private void setLimit()
    {
        if(_selectedBullet != 0)
        {
            for (int i = 0; i < _bulletsLimit.Count; i++)
            {
                var current = _bulletsLimit[i];                
                if (_selectedBullet == current.Item1)
                {
                    var newValue = (current.Item1, current.Item2 - 1);
                    if (newValue.Item2 <= 0)
                        newValue.Item2 = 0;
                    _bulletsLimit[i] = newValue;
                }
            }
            Set_Limit_UI();
        }              
    }

    private int getLimit()
    {
        if(_selectedBullet != 0)
        {
            for (int i = 0; i < _bulletsLimit.Count; i++)
            {
                var current = _bulletsLimit[i];
                if (_selectedBullet == current.Item1)
                {
                    return current.Item2;
                }
            }
        }
        return 0;
    }

    public void Set_Limit_UI()
    {        
        List<int> itemsToRemove = new List<int>();

        foreach (var el in _bulletsLimit)
        {
            if (UI_Controller.instance.bullet_slot_1.GetComponent<Bullet_Slot>().index == el.Item1)
            {
                UI_Controller.instance.Bullet_Limit_1.text = el.Item2.ToString();
                if (el.Item2 == 0)
                {
                    UI_Controller.instance.Select_Bullet_1.gameObject.SetActive(false);
                    UI_Controller.instance.ResetChecks();
                    UI_Controller.instance.Checks[0].SetActive(true);
                    itemsToRemove.Add(el.Item1);
                }
            }
            if (UI_Controller.instance.bullet_slot_2.GetComponent<Bullet_Slot>().index == el.Item1)
            {
                UI_Controller.instance.Bullet_Limit_2.text = el.Item2.ToString();
                if (el.Item2 == 0)
                {
                    UI_Controller.instance.Select_Bullet_2.gameObject.SetActive(false);
                    UI_Controller.instance.ResetChecks();
                    UI_Controller.instance.Checks[0].SetActive(true);
                    itemsToRemove.Add(el.Item1);
                }
            }
            if (UI_Controller.instance.bullet_slot_Extra.GetComponent<Bullet_Slot>().index == el.Item1)
            {
                UI_Controller.instance.Bullet_Limit_Extra.text = el.Item2.ToString();
                if (el.Item2 == 0)
                {
                    UI_Controller.instance.Select_Bullet_Extra.gameObject.SetActive(false);
                    UI_Controller.instance.ResetChecks();
                    UI_Controller.instance.Checks[0].SetActive(true);
                    itemsToRemove.Add(el.Item1);
                }
            }
        }

        foreach (int itemToRemove in itemsToRemove)
        {
            _bulletsLimit.RemoveAll(tuple => tuple.Item1 == itemToRemove);
        }
    }
}