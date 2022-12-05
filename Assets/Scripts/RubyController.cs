using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class RubyController : MonoBehaviour
{
    public static RubyController instance { get; private set; }
    Animator animator;
    Rigidbody2D rb;
    float horizontal;
    float vertical;
    Vector2 lookDirection = new Vector2(1, 0);

    public float speed = 3.0f;

    public int maxHealth = 5;
    float timeInvincible = 2.0f;

    public int health { get { return currentHealth; } }
    int currentHealth;

    int ammo;
    int startAmmo;
    public TextMeshProUGUI ammoCount;
    int pickedUpCogs;

    public TextMeshProUGUI woodCount;
    public int wood;

    bool isInvincible;
    float invincibleTimer;

    AudioSource audioSource;
    public AudioClip throwSound;
    public AudioClip hitSound;
    public AudioClip healSound;

    public GameObject projectilePrefab;
    public ParticleSystem hitEffect;
    public ParticleSystem healEffect;


    public bool loseCondition = false;
    public bool winCondition = false;

    public TextMeshProUGUI endText;

    public bool spokeToJambi;

    public static Scene scene;
    public string currentScene;

    void Awake()
    {
        instance = this;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        endText.text = "";

        startAmmo = 10;
        ammo = startAmmo;
        ammoCount.text = ammo.ToString();

        woodCount.text = "Wood: " + wood.ToString();

        currentHealth = maxHealth;

        spokeToJambi = false;

        scene = SceneManager.GetActiveScene();
        currentScene = scene.name;
        StageController.inst.StageSettings(currentScene);
    }


    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        Vector2 move = new Vector2(horizontal, vertical);

        if (!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
        {
            lookDirection.Set(move.x, move.y);
            lookDirection.Normalize();
        }
        if (!winCondition && !loseCondition)
        {
            animator.SetFloat("Look X", lookDirection.x);
            animator.SetFloat("Look Y", lookDirection.y);
        }

        animator.SetFloat("Speed", move.magnitude);

        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;

            if (invincibleTimer < 0)
            {
                isInvincible = false;
            }
        }


        if (Input.GetKeyDown(KeyCode.C))
        {
            Launch();
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            RaycastHit2D hit = Physics2D.Raycast(rb.position + Vector2.up * 0.2f, lookDirection, 1.5f, LayerMask.GetMask("NPC"));
            if (hit.collider != null)
            {

                NPC character = hit.collider.GetComponent<NPC>();
                if (character != null)
                {
                    character.DisplayDialog();
                    if (FixedCounter.instance.fixedCount == FixedCounter.instance.totalCount)
                    {
                        spokeToJambi = true;
                        StageController.inst.currentStage++;
                        StageController.inst.StageSelect();
                    }
                }
            }
            
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            RaycastHit2D hitBridge = Physics2D.Raycast(rb.position + Vector2.left * 0.2f, lookDirection, 1.5f, LayerMask.GetMask("Bridge"));
            if (hitBridge.collider != null)
            {     
                Bridge bridge = hitBridge.collider.GetComponent<Bridge>();
                if (bridge != null && wood >= 10 && !bridge.isFixed)
                {
                    Debug.Log("It worked");
                    bridge.FixBridge();
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        if (ammo + pickedUpCogs + StageController.inst.totalCogPickups <= 0 || currentHealth <= 0 && !loseCondition) //sets game to lose state
        {
            loseCondition = true;
            End();
        }

        if (FixedCounter.instance.fixedCount == FixedCounter.instance.totalCount && !winCondition)
        {
            if (StageController.inst.currentStage == 0 && !spokeToJambi)
            {
                return;
            }

            winCondition = true;
            Debug.Log("Testing WINCONDITION " + winCondition);
            if (StageController.inst.currentStage != 0)
            {
                End();
            }

        }

        if (winCondition && Input.GetKeyDown(KeyCode.R) || loseCondition && Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene("Scene0");
        }

    }

    void FixedUpdate()
    {
        if (!winCondition && !loseCondition)
        {
            Vector2 position = transform.position;
            position.x = position.x + speed * horizontal * Time.deltaTime;
            position.y = position.y + speed * vertical * Time.deltaTime;
            rb.MovePosition(position);
        }
    }

    public void ChangeHealth(int amount)
    {
        if (amount < 0)
        {

            if (isInvincible) return;

            animator.SetTrigger("Hit");

            invincibleTimer = timeInvincible;
            isInvincible = true;
            if (health > 0)
            {
                ParticleSystem hit = Instantiate(hitEffect, rb.position, Quaternion.identity);
                PlaySound(hitSound);
            }


        }
        if (amount > 0)
        {
            ParticleSystem heal = Instantiate(healEffect, rb.position, Quaternion.identity);
            PlaySound(healSound);
        }

        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        UIHealthBar.instance.SetValue(currentHealth / (float)maxHealth);
    }

    void Launch()
    {
        if (ammo > 0)
        {
            CogAmount(-1);

            GameObject projectileObject = Instantiate(projectilePrefab, rb.position + Vector2.up * 0.5f, Quaternion.identity);

            Projectile projectile = projectileObject.GetComponent<Projectile>();
            projectile.Launch(lookDirection, 300);
            animator.SetTrigger("Launch");
            PlaySound(throwSound);
        }
    }
    public void CogAmount(int change)
    {
        ammo = ammo + change;
        ammoCount.text = ammo.ToString();
        if (change > 0)
        {
            pickedUpCogs = pickedUpCogs - change;
        }
    }

    public void WoodAmount(int change)
    {
        wood = wood + change;
        woodCount.text = "Wood: "+ wood.ToString();

    }

    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }

    public void End()
    {
        audioSource.Stop();
        animator.SetFloat("Speed", 0);
        if (loseCondition)
        {
            if (currentHealth <= 0)
            {
                endText.text = "Oh no! You died. Press 'R' to restart";
            }
            else if (ammo + pickedUpCogs + StageController.inst.totalCogPickups <= 0)
            {
                endText.text = "Oh no! You don't have enough cogs to fix the robots. Press 'R' to restart";          
            }
        }

        if (winCondition)
        {
            endText.text = "Great Job! You fixed all the robots! Press 'R' to restart. - Game created by Max Jones";
        }

    }

    public void TalkToJambi()
    {
        endText.text = "Good job! You fixed the robots. Talk to Jambi to move on.";
    }

}
